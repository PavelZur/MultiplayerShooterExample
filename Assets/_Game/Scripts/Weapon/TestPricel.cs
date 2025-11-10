using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;


public class TestPricel : MonoBehaviour
{
    [SerializeField] private Transform pointNear;
    [SerializeField] private Transform pointFar;
    [SerializeField] private Transform _proxyCamera;

    Camera camera;
    Transform weaponTransform;

    Vector3 oldPos;
    Quaternion oldRot;

    bool _isPricel;
    void Start()
    {
        camera = Camera.main;
        weaponTransform = transform;
        oldPos = weaponTransform.localPosition;
        oldRot = weaponTransform.localRotation;
    }

    private CancellationTokenSource _cts;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!_isPricel)
            {
                _isPricel = true;
                // AlignWeaponToAim();
                _cts = new CancellationTokenSource();
                AlignWeaponCoroutineAsync(_cts.Token,alignSpeed).Forget();
            }
            else
            {
                _isPricel = false;
                camera.fieldOfView = 60;
                weaponTransform.SetLocalPositionAndRotation(oldPos, oldRot);
            }

        }
        // AlignWeaponToAim();
    }

    void AlignWeaponToAim()
    {
        if (!_isPricel) return;

        Vector3 forwardInWorldSpace = _proxyCamera.transform.rotation * Vector3.forward * 0.5f;
        Vector3 proxyCameraPosInWorld = _proxyCamera.transform.position;
        Vector3 targetPosInWorld = proxyCameraPosInWorld + forwardInWorldSpace;

        targetPosInWorld = weaponTransform.parent.InverseTransformPoint(targetPosInWorld);
        Vector3 offset = targetPosInWorld - weaponTransform.parent.InverseTransformPoint(pointNear.position);

        weaponTransform.localPosition += offset;

        Vector3 nearToFar = pointFar.localPosition - pointNear.localPosition;
        if (nearToFar.sqrMagnitude < 0.0001f)
            return;

        Vector3 desiredDir = weaponTransform.InverseTransformDirection(_proxyCamera.forward);

        Quaternion rot = Quaternion.FromToRotation(nearToFar.normalized, desiredDir);

        Vector3 worldNearBefore = pointNear.position;

        weaponTransform.localRotation = rot * oldRot;

        Vector3 worldNearAfter = pointNear.position;
        Vector3 correction = worldNearBefore - worldNearAfter;

        weaponTransform.position += correction;
    }




    public float alignSpeed = 5;
    private async UniTask AlignWeaponCoroutineAsync(CancellationToken token, float duration = 1)
    {
        if (!_isPricel)
            return;

        float startFieldView = camera.fieldOfView;
        float endtFieldView = 35f;

        var proxyCam = _proxyCamera.transform;
        Vector3 targetPosInWorld = proxyCam.position + proxyCam.rotation * Vector3.forward * 0.5f;
        targetPosInWorld = weaponTransform.parent.InverseTransformPoint(targetPosInWorld);
        Vector3 offset = targetPosInWorld - weaponTransform.parent.InverseTransformPoint(pointNear.position);

        Vector3 startLocalPos = weaponTransform.localPosition;
        Vector3 endLocalPos = startLocalPos + offset;

        Vector3 nearToFar = pointFar.localPosition - pointNear.localPosition;
        if (nearToFar.sqrMagnitude < 1e-4f)
            return;

        Vector3 desiredDir = weaponTransform.InverseTransformDirection(proxyCam.forward);
        Quaternion rot = Quaternion.FromToRotation(nearToFar.normalized, desiredDir);
        Quaternion startRot = weaponTransform.localRotation;
        Quaternion endRot = rot * oldRot;

        Vector3 worldNearBefore = pointNear.position;

        weaponTransform.localRotation = endRot;
        Vector3 worldNearAfter = pointNear.position;
        weaponTransform.localRotation = startRot;

        Vector3 correctionLocal = weaponTransform.parent.InverseTransformVector(worldNearBefore - worldNearAfter);
        endLocalPos += correctionLocal;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (token.IsCancellationRequested)
                return;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            weaponTransform.localPosition = Vector3.Lerp(startLocalPos, endLocalPos, t);
            weaponTransform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            camera.fieldOfView = Mathf.Lerp(startFieldView,endtFieldView,t);

            await UniTask.Yield(token);
        }

        weaponTransform.localPosition = endLocalPos;
        weaponTransform.localRotation = endRot;
    }
}

    
