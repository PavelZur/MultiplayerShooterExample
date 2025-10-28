using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShootingController : MonoBehaviour
{
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] private Pistol _pistol;
    [SerializeField] private GunAnimation _gunAnimation;
    //пока что тут только пистолет, немного грязновато на скорую, потом переделается под разные виды оружия , типы патронов и пушек с разными параметрами

    private void Start()
    {
        _pistol.ReloadGun += () =>
        {
            _gunAnimation.PlayReloadAnim();
            MultiplayerManager.Instance.SendMessageColyseus("reloadgun");
        };
    }

    void Update()
    {
        if (InputController.Instance.MouseLeftButtonPressed)
        {
            if (!_pistol.TryShoot()) return;

            Hit().Forget();
        }
    }

    private async UniTaskVoid Hit()
    {
        Bullet newBullet = Instantiate(_pistol.PrefabBullet, _pistol.BulletStartTranform.position, _pistol.BulletStartTranform.rotation);

        Ray ray = new(_pistol.BulletStartTranform.position, _pistol.BulletStartTranform.forward);

        Vector3 bulletTarget = _pistol.BulletStartTranform.position + (ray.direction * 100f);

        Health health = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerMask))
        {
            hit.collider.TryGetComponent(out health);
            bulletTarget = hit.point;
        }

        SendBulletInfo(bulletTarget);


        _gunAnimation.PlayShootAnim();


        await newBullet.BulletFlight(_pistol.BulletStartTranform.position, bulletTarget, _pistol.BulletSpeed);

        if (health != null)
        {
            health.RemoveHealth(_pistol.Damage);
        }
    }

    private void SendBulletInfo(Vector3 bulletTarget)
    {
        ShootingInfo info = new()
        {
            key = MultiplayerManager.Instance.PlayerID,

            stX = _pistol.BulletStartTranform.position.x,
            stY = _pistol.BulletStartTranform.position.y,
            stZ = _pistol.BulletStartTranform.position.z,

            tarX = bulletTarget.x,
            tarY = bulletTarget.y,
            tarZ = bulletTarget.z,

            bspeed = _pistol.BulletSpeed
        };

        string data = JsonUtility.ToJson(info);
        MultiplayerManager.Instance.SendMessageColyseus("shoot", data);
    }
}

[System.Serializable]
public struct ShootingInfo
{
    public float key;

    public float stX;
    public float stY;
    public float stZ;

    public float tarX;
    public float tarY;
    public float tarZ;

    public float bspeed;
}
