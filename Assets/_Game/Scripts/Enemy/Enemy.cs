using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private EnemyDataReceiver _enemyDataReceiver;
    [SerializeField] private PlayerWeaponModel _weaponModel;
    [SerializeField] private EnemyWeaponController _weaponController;
    [SerializeField] private EnemyController _enemyController;
    public string SessionId { get; private set; }

    public async UniTaskVoid Init(Player player, string sessionId)
    {
        SessionId = sessionId;

        Vector3 pos = new(player.movementData.px, player.movementData.py, player.movementData.pz);
        Vector3 vel = new(player.movementData.vx, player.movementData.vy, player.movementData.vz);
        float rotY = player.movementData.ry;
        float rotX = player.movementData.rx;

        _enemyController.Init(pos, vel, rotY, rotX);
        _enemyDataReceiver.InitDataReceiver(player, sessionId);
        _health.Init(player.healthData.maxHealth, player.healthData.curHealth);

        await UniTask.WaitUntil(() => _weaponController.IsInit);
        _weaponModel.CurrentActiveWeapon.Value = (TypeWeapon)player.weaponData.weapon;
    }

    public void Kill()
    {
        //пока тут реализация супер простая , смерти как таковой нет , тупо дестрой.
        Destroy(gameObject);
    }
}
