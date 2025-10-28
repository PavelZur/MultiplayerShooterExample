using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerMovementSyncer : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _movementModel;

    private const string KEY_MOVE = "move";
    private readonly Dictionary<string, object> _movementInfoDictionaty = new()
    {
        { "px", 0f },
        { "py", 0f },
        { "pz", 0f },
        { "vx", 0f },
        { "vy", 0f },
        { "vz", 0f },
        { "ry", 0f },
        { "rx", 0f },

        { "ground", 0f },
        { "anspeed", 0f },
        { "sit", 0f },

    };

    private void Start()
    {
        SendPosAndRot(transform.position);
    }

    private Vector3 ExtrapolationPositionOnPing(Vector3 cerrentPos)
    {
        float delaySec = (MultiplayerManager.Instance.RTT / 2) / 1000;
        return cerrentPos + (_movementModel.PlayerVelosity.Value * delaySec);
    }

    private void FixedUpdate()
    {
        SendPosAndRot(_movementModel.PlayerPosition.Value);
    }

    private void SendPosAndRot(Vector3 pos)
    {
        Vector3 predictPosition = ExtrapolationPositionOnPing(pos);

        _movementInfoDictionaty["px"] = predictPosition.x;
        _movementInfoDictionaty["py"] = predictPosition.y;
        _movementInfoDictionaty["pz"] = predictPosition.z;

        _movementInfoDictionaty["vx"] = _movementModel.PlayerVelosity.Value.x == 0? 0.01 : _movementModel.PlayerVelosity.Value.x;
        _movementInfoDictionaty["vy"] = _movementModel.PlayerVelosity.Value.y == 0? 0.01 : _movementModel.PlayerVelosity.Value.y;
        _movementInfoDictionaty["vz"] = _movementModel.PlayerVelosity.Value.z == 0? 0.01 : _movementModel.PlayerVelosity.Value.z;

        _movementInfoDictionaty["ry"] = _movementModel.PlayerRotationY.Value;
        _movementInfoDictionaty["rx"] = _movementModel.HandRotationX.Value;

        _movementInfoDictionaty["ground"] = _movementModel.IsGrounded.Value == true ? 1 : 0;
        _movementInfoDictionaty["sit"] = _movementModel.IsSitting.Value == true ? 1 : 0;
        _movementInfoDictionaty["anspeed"] = _movementModel.Speed.Value;

        //  Debug.Log("vy    " + _movementInfoDictionaty["vy"]);

        MultiplayerManager.Instance.SendMessageColyseus(KEY_MOVE, _movementInfoDictionaty);
    }
}
