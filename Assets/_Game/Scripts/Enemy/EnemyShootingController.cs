using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private GunAnimation _gunAnimation;
    [SerializeField] private Bullet _bulletPref;
    [SerializeField] private Transform _spawnPoint;
    void OnEnable()
    {
        MultiplayerManager.Instance.OnShootingEnemyEvent += Shoot;
    }

    private void Shoot(ShootingInfo info)
    {
        Vector3 start = new(info.stX, info.stY, info.stZ);
        Vector3 target = new(info.tarX, info.tarY, info.tarZ);
        Bullet newBullet = Instantiate(_bulletPref, start, _spawnPoint.rotation);
        newBullet.BulletFlight(start, target, info.bspeed).Forget();
        _gunAnimation.PlayShootAnim();
    }

    private void OnDisable()
    {
        MultiplayerManager.Instance.OnShootingEnemyEvent -= Shoot;
    }
}
