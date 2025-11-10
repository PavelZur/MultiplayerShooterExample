using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AimingStateController : MonoBehaviour
{
    [SerializeField] private ProxyCamera _proxyCamera;
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private Transform _inconAimTransform;

    private Camera _camera;
    private float _cameraFieldStandart = 60;
    private bool _isAiming;
    private CancellationTokenSource _cts;

    private void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        // пока тестовый вариант, нету защиты на выход из режима при смене оружия

        if (Input.GetMouseButtonDown(1))
        {
            WeaponAim cerentAim = _weaponController.CurrentActiveWeapon.ActivAim;
            Transform weaponTransform = _weaponController.CurrentActiveWeapon.RootMeshTransform;

            if (!_isAiming)
            {
                
                _isAiming = true;
                _cts = new CancellationTokenSource();
                AlignWeaponAsync(cerentAim.PointNear, cerentAim.PointFar,
                    weaponTransform.localRotation, cerentAim.OffsetOnCamera,
                    cerentAim.TimeToAiming , cerentAim, weaponTransform, _cts.Token).Forget();
            }
            else
            {
                _isAiming = false;
                _cts?.Cancel();
                _camera.fieldOfView = _cameraFieldStandart;
                _inconAimTransform.localScale = Vector3.one;
                weaponTransform.SetLocalPositionAndRotation(cerentAim.StartLocalPosition, cerentAim.StartLocalRotation);
            }
        }
    }

    // пока тестово 
    private async UniTask AlignWeaponAsync(Transform pointNear, Transform pointFar,
        Quaternion cerentLocalWeaponRot, float distance, float duration, WeaponAim cerentAim, Transform transformWeapon ,CancellationToken token)
    {
        if (!_isAiming)
            return;

        float startFieldView = _camera.fieldOfView;
        float endtFieldView = cerentAim.FieldView;

        var proxyCam = _proxyCamera.transform;
        Vector3 targetPosInWorld = proxyCam.position + proxyCam.rotation * Vector3.forward * distance;
        targetPosInWorld = transformWeapon.parent.InverseTransformPoint(targetPosInWorld);
        Vector3 offset = targetPosInWorld - transformWeapon.parent.InverseTransformPoint(pointNear.position);

        Vector3 startLocalPos = transformWeapon.localPosition;
        Vector3 endLocalPos = startLocalPos + offset;

        Vector3 nearToFar = pointFar.localPosition - pointNear.localPosition;
        if (nearToFar.sqrMagnitude < 1e-4f)
            return;

        Vector3 desiredDir = transformWeapon.InverseTransformDirection(proxyCam.forward);
        Quaternion rot = Quaternion.FromToRotation(nearToFar.normalized, desiredDir);
        Quaternion startRot = transformWeapon.localRotation;
        Quaternion endRot = rot * cerentLocalWeaponRot;

        Vector3 worldNearBefore = pointNear.position;
        transformWeapon.localRotation = endRot;
        Vector3 worldNearAfter = pointNear.position;
        transformWeapon.localRotation = startRot;

        Vector3 correctionLocal = transformWeapon.parent.InverseTransformVector(worldNearBefore - worldNearAfter);
        endLocalPos += correctionLocal;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (token.IsCancellationRequested)
                return;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            _inconAimTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            transformWeapon.localPosition = Vector3.Lerp(startLocalPos, endLocalPos, t);
            transformWeapon.localRotation = Quaternion.Slerp(startRot, endRot, t);
            _camera.fieldOfView = Mathf.Lerp(startFieldView, endtFieldView, t);

            await UniTask.Yield(token);
        }

        _inconAimTransform.localScale = Vector3.zero;
        transformWeapon.localPosition = endLocalPos;
        transformWeapon.localRotation = endRot;
    }
}
