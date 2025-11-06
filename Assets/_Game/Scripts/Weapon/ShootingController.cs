using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] protected LayerMask _layerMask;

    public Action<Vector3> OnShootEvent;
    public Action<string,int,bool> OnApplyEnemyDamageEvent;

    private CancellationTokenSource _shootingCancellationTokenSource;

    void Update()
    {
        if (InputController.Instance.MouseLeftButtonDown)
        {
            if (_weaponController.IsChangeWeaponProccess ) return;

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

        Ray ray = new(_weaponController.CurrentActiveWeapon.BulletStartTranform.position,
            _weaponController.CurrentActiveWeapon.BulletStartTranform.forward);

        Vector3 bulletTarget = _weaponController.CurrentActiveWeapon.BulletStartTranform.position + (ray.direction * 100f);

        LimbDamageCorrection health = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerMask))
        {
            hit.collider.TryGetComponent(out health);
            bulletTarget = hit.point;
        }

        OnShootEvent?.Invoke(bulletTarget);

        newBullet.gameObject.SetActive(true);
        await newBullet.BulletFlight(_weaponController.CurrentActiveWeapon.BulletStartTranform.position, bulletTarget,
            _weaponController.CurrentActiveWeapon.WeaponParametrs.BulletSpeed);

        if (health != null)
        {
            OnApplyEnemyDamageEvent?.Invoke(health.SessionId,
                (int)(_weaponController.CurrentActiveWeapon.WeaponParametrs.Damage * health.MultipleFactorDamage),
                health.IsHead);
        }

        await UniTask.Delay(1000); _weaponController.CurrentActiveWeapon.BulletPoolPrefabs.ReturnBullet(newBullet);
    }
}


