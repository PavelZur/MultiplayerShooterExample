using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] protected LayerMask _layerMask;

    public Action<Vector3,Vector3,byte> OnShootEventOnSync;
    public Action OnShootEvent;
    public Action<string, int, bool> OnApplyEnemyDamageEvent;

    private CancellationTokenSource _shootingCancellationTokenSource;
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (InputController.Instance.MouseLeftButtonDown)
        {
            if (_weaponController.IsChangeWeaponProccess) return;

            if (_weaponController.CurrentActiveWeapon.WeaponParametrs.IsAutomatic)
            {
                StartAutomaticShooting();
            }

            else
            {
                if (_weaponController.CurrentActiveWeapon.TryAmmo())
                {
                    OneShoot().Forget();
                }
            }
        }

        if (InputController.Instance.MouseLeftButtonUp)
        {
            StopAutomaticShooting();
        }
    }

    private void StartAutomaticShooting()
    {
        if (_shootingCancellationTokenSource != null) return;

        _shootingCancellationTokenSource = new CancellationTokenSource();
        AutomaticShooting(_shootingCancellationTokenSource.Token).Forget();
    }

    private void StopAutomaticShooting()
    {
        if (_shootingCancellationTokenSource != null)
        {
            _shootingCancellationTokenSource.Cancel();
            _shootingCancellationTokenSource = null;
        }
    }

    private async UniTaskVoid AutomaticShooting(CancellationToken ct)
    {
        float period = _weaponController.CurrentActiveWeapon.WeaponParametrs.PeriodShooting;

        while (!ct.IsCancellationRequested)
        {
            if (!_weaponController.CurrentActiveWeapon.TryAmmo())
            {
                StopAutomaticShooting();
                break;
            }

            OneShoot().Forget();

            await UniTask.Delay(TimeSpan.FromSeconds(period), cancellationToken: ct);
        }
    }

    private async UniTaskVoid OneShoot()
    {
        Bullet newBullet = _weaponController.CurrentActiveWeapon.BulletPoolPrefabs.GetBullet();

        newBullet.transform.SetPositionAndRotation(_weaponController.CurrentActiveWeapon.BulletStartTranform.position,
            _weaponController.CurrentActiveWeapon.BulletStartTranform.rotation);

        Ray ray = new(_cameraTransform.position,
            _cameraTransform.forward);

        Vector3 bulletTarget = _cameraTransform.position + (ray.direction * 100f);

        LimbDamageCorrection health = null;
        DecalBulletType _decalBulletType = DecalBulletType.None;
        Vector3 normal = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerMask))
        {
            hit.collider.TryGetComponent(out health);
            bulletTarget = hit.point;

            int hitLayer = hit.collider.gameObject.layer;
            normal = hit.normal;

            _decalBulletType = hitLayer switch
            {
                6 => DecalBulletType.Blood,
                9 => DecalBulletType.Stone,
                10 => DecalBulletType.Metall,
                11 => DecalBulletType.Wood,
                12 => DecalBulletType.Glass,
                13 => DecalBulletType.Grass,
                _ => DecalBulletType.None,
            };
        }

        OnShootEventOnSync?.Invoke(bulletTarget,normal, (byte)_decalBulletType);
        OnShootEvent?.Invoke();

        newBullet.gameObject.SetActive(true);
        await newBullet.BulletFlight(_weaponController.CurrentActiveWeapon.BulletStartTranform.position, bulletTarget,
            _weaponController.CurrentActiveWeapon.WeaponParametrs.BulletSpeed, _decalBulletType, normal);

        if (health != null)
        {
            OnApplyEnemyDamageEvent?.Invoke(health.SessionId,
                (int)(_weaponController.CurrentActiveWeapon.WeaponParametrs.Damage * health.MultipleFactorDamage),
                health.IsHead);
        }

        await UniTask.Delay(1000); _weaponController.CurrentActiveWeapon.BulletPoolPrefabs.ReturnBullet(newBullet);
    }
}

public enum DecalBulletType
{
    None,
    Blood,
    Stone,
    Metall,
    Wood,
    Glass,
    Grass
}


