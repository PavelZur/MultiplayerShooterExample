using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyWeaponController _enemyWeaponController;
    [SerializeField] private EnemyDataReceiver _enemyDataReceiver;
    public Action OnShootViewEvent;
    Action<ShootingInfo> Handler;
    private void Start()
    {
        Handler = (info) => Shoot(info).Forget();
        _enemyDataReceiver.Shoot += Handler;
      // _enemyDataReceiver.Shoot += (info) => Shoot(info).Forget();
    }

    private async UniTaskVoid Shoot(ShootingInfo info)
    {
        Vector3 start = _enemyWeaponController.CurrentActiveWeapon.BulletStartTranform.position;
        Vector3 target = new(info.tarX, info.tarY, info.tarZ);

        DecalBulletType decalBulletType = (DecalBulletType)info.type;
        Vector3 normal = new(info.norX, info.norY, info.norZ); ;

        Bullet newBullet = _enemyWeaponController.CurrentActiveWeapon.BulletPoolPrefabs.GetBullet();

        newBullet.transform.SetPositionAndRotation(start, _enemyWeaponController.CurrentActiveWeapon.BulletStartTranform.rotation);

        newBullet.gameObject.SetActive(true);
        OnShootViewEvent?.Invoke();
        newBullet.BulletFlight(start, target, _enemyWeaponController.CurrentActiveWeapon.WeaponParametrs.BulletSpeed, decalBulletType, normal).Forget();
        await UniTask.Delay(1000);
        _enemyWeaponController.CurrentActiveWeapon.BulletPoolPrefabs.ReturnBullet(newBullet);
    }

    private void OnDestroy()
    {
        _enemyDataReceiver.Shoot -= Handler;
    }
}

