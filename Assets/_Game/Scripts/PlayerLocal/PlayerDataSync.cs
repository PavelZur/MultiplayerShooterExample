using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

public class PlayerDataSync : MonoBehaviour
{
    [SerializeField] private ShootingController _shootingController;
    [SerializeField] private PlayerMovementModel _movementModel;
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private PlayerWeaponModel _weaponModel;

    private const string KEY_STATE_MOVEMENT = "statemovement";
    private const string KEY_CHANGE_WEAPON = "weaponchange";
    private const string KEY_RELOAD_WEAPON = "reloadweapon";
    private const string KEY_APPLY_DAMAGE = "applydamage";
    private const string KEY_SHOOT = "shoot";
    private const string KEY_MOVE = "move";

    private readonly Dictionary<string, object> _movementInfoData = new()
    {
        { "px", 0f },
        { "py", 0f },
        { "pz", 0f },
        { "vx", 0f },
        { "vy", 0f },
        { "vz", 0f },
        { "ry", 0f },
        { "rx", 0f },
    };

    private readonly Dictionary<string, object> _stateMovementData = new()
    {
        { "sit", false },
    };

    private readonly Dictionary<string, object> _weaponData = new()
    {
        { "id", 0 }
    };

    private readonly Dictionary<string, object> _damageData = new()
    {
        { "id", "" },
        { "health", 0 }
    };


    private void Start()
    {
        SendPosAndRot(transform.position);

        _movementModel.IsSitting.Subscribe(isSiting => SendStateMovement(isSiting)).AddTo(this);
        _weaponModel.CurrentActiveWeapon.Subscribe(weapon => SendActivWeapon(weapon)).AddTo(this);

        _weaponController.OnReloadGunEvent += SendReloadWeapon;
        _shootingController.OnShootEvent += SendBulletInfo;
        _shootingController.OnApplyEnemyDamageEvent += SendApplyDamageEnemy;
    }

    private void FixedUpdate()
    {
        SendPosAndRot(_movementModel.PlayerPosition.Value);
    }

    private Vector3 ExtrapolationPositionOnPing(Vector3 cerrentPos)
    {
        float delaySec = (MultiplayerManager.Instance.RTT / 2) / 1000;
        return cerrentPos + (_movementModel.PlayerVelosity.Value * delaySec);
    }

    private void SendPosAndRot(Vector3 pos)
    {
        Vector3 predictPosition = ExtrapolationPositionOnPing(pos);

        _movementInfoData["px"] = predictPosition.x;
        _movementInfoData["py"] = predictPosition.y;
        _movementInfoData["pz"] = predictPosition.z;

        _movementInfoData["vx"] = _movementModel.PlayerVelosity.Value.x == 0 ? 0.01 : _movementModel.PlayerVelosity.Value.x;
        _movementInfoData["vy"] = _movementModel.PlayerVelosity.Value.y == 0 ? 0.01 : _movementModel.PlayerVelosity.Value.y;
        _movementInfoData["vz"] = _movementModel.PlayerVelosity.Value.z == 0 ? 0.01 : _movementModel.PlayerVelosity.Value.z;

        _movementInfoData["ry"] = _movementModel.PlayerRotationY.Value;
        _movementInfoData["rx"] = _movementModel.HandRotationX.Value;

        MultiplayerManager.Instance.SendMessageColyseus(KEY_MOVE, _movementInfoData);
    }

    private void SendStateMovement(bool value)
    {
        _stateMovementData["sit"] = value;
        MultiplayerManager.Instance.SendMessageColyseus(KEY_STATE_MOVEMENT, _stateMovementData);
    }

    private void SendActivWeapon(TypeWeapon typeWeapon)
    {
        _weaponData["id"] = (byte)typeWeapon;
        MultiplayerManager.Instance.SendMessageColyseus(KEY_CHANGE_WEAPON, _weaponData);
    }

    private void SendReloadWeapon()
    {
        MultiplayerManager.Instance.SendMessageColyseus(KEY_RELOAD_WEAPON);
    }

    private void SendBulletInfo(Vector3 bulletTarget)
    {
        ShootingInfo info = new()
        {
            key = MultiplayerManager.Instance.PlayerID,

            tarX = bulletTarget.x,
            tarY = bulletTarget.y,
            tarZ = bulletTarget.z,
        };

        string data = JsonUtility.ToJson(info);
        MultiplayerManager.Instance.SendMessageColyseus(KEY_SHOOT, data);
    }

    private void SendApplyDamageEnemy(string enemySessionId, int damage)
    {
        _damageData["id"] = enemySessionId;
        _damageData["damage"] = damage;

        MultiplayerManager.Instance.SendMessageColyseus(KEY_APPLY_DAMAGE, _damageData);
    }

    [Button]
    public void SendApplyDamageEnemy()
    {
        _damageData["id"] = MultiplayerManager.Instance.PlayerID;
        _damageData["damage"] = 10;

        MultiplayerManager.Instance.SendMessageColyseus(KEY_APPLY_DAMAGE, _damageData);
    }

    private void OnDestroy()
    {
        _weaponController.OnReloadGunEvent -= SendReloadWeapon;
        _shootingController.OnShootEvent -= SendBulletInfo;
        _shootingController.OnApplyEnemyDamageEvent -= SendApplyDamageEnemy;
    }
}
