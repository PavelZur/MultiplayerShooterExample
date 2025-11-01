using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyWeaponController _enemyWeaponController;
    [SerializeField] private EnemyDataReceiver _enemyDataReceiver;
    public Action OnShootViewEvent;

    private void Start()
    {
        _enemyDataReceiver.Shoot += Shoot;
    }
    private void Shoot(ShootingInfo info)
    {
        Vector3 start = _enemyWeaponController.CurrentActiveWeapon.BulletStartTranform.position;
        Vector3 target = new(info.tarX, info.tarY, info.tarZ);

        Bullet newBullet = Instantiate(_enemyWeaponController.CurrentActiveWeapon.WeaponParametrs.PrefabBullet,
            start, _enemyWeaponController.CurrentActiveWeapon.BulletStartTranform.rotation);

        newBullet.BulletFlight(start, target, _enemyWeaponController.CurrentActiveWeapon.WeaponParametrs.BulletSpeed).Forget();
        OnShootViewEvent?.Invoke();
    }

    private void OnDestroy()
    {
        _enemyDataReceiver.Shoot -= Shoot;
    }
}

