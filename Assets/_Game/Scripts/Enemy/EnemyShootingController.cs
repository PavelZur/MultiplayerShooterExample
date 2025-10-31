using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyWeaponController _enemyWeaponController;
    public Action OnShootEvent;


    void OnEnable()
    {
        MultiplayerManager.Instance.OnShootingEnemyEvent += Shoot;
    }

    private void Shoot(ShootingInfo info)
    {

        if (info.key != enemy.SessionId) return;

        Vector3 start = _enemyWeaponController.CurrentActiveWeapon.BulletStartTranform.position;
        Vector3 target = new(info.tarX, info.tarY, info.tarZ);

        Bullet newBullet = Instantiate(_enemyWeaponController.CurrentActiveWeapon.WeaponParametrs.PrefabBullet,
            start, _enemyWeaponController.CurrentActiveWeapon.BulletStartTranform.rotation);

        newBullet.BulletFlight(start, target, _enemyWeaponController.CurrentActiveWeapon.WeaponParametrs.BulletSpeed).Forget();
        OnShootEvent?.Invoke();
    }

    private void OnDisable()
    {
        MultiplayerManager.Instance.OnShootingEnemyEvent -= Shoot;
    }
}
