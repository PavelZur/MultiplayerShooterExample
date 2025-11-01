using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;
using System;

public class EnemyDataReceiver : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private PlayerMovementModel _movementModel;
    [SerializeField] private PlayerWeaponModel _weaponModel;
    [SerializeField] private Health _health;
    public Action<ShootingInfo> Shoot;
    public Action ReloadWeapon;
    public Action Die;
    private string _sessionId;

    public void InitDataReceiver(Player player ,string sessionId)
    {
        _sessionId = sessionId;
        player.healthData.OnChange += OnChangeHealthOnServer;
        player.movementData.OnChange += OnChangeMovementOnServer;
        player.movementStateData.OnChange += OnChangeStateMovementOnServer;
        player.weaponData.OnChange += OnChangeWeaponOnServer;
        player.playerStatedata.OnChange += OnChangePlayerStateOnServer;
        MultiplayerManager.Instance.OnShootingEnemyEvent += OnShootMessageHandler;
        MultiplayerManager.Instance.OnEnemyGunReloadEvent += OnReloadWeaponMessageHandler;
    }

    private void OnChangePlayerStateOnServer(List<DataChange> changes)
    {
        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "die":
                    _movementModel.IsDieState.Value = (bool)dataChanges.Value;
                    Die?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

    private void OnShootMessageHandler(ShootingInfo info)
    {
        if (info.key != _sessionId) return;
        Shoot?.Invoke(info);
    }

    private void OnReloadWeaponMessageHandler(string sessionId)
    {
        if (sessionId != _sessionId) return;
        ReloadWeapon?.Invoke();
    }

    public void OnChangeMovementOnServer(List<DataChange> changes)
    {
        _enemyController.OnChangeHandler(changes);
    }

    public void OnChangeStateMovementOnServer(List<DataChange> changes)
    {
        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "sit":
                    _movementModel.IsSitting.Value = (bool)dataChanges.Value;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnChangeWeaponOnServer(List<DataChange> changes)
    {

        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "weapon":
                    byte index = (byte)dataChanges.Value;
                    _weaponModel.CurrentActiveWeapon.Value = (TypeWeapon)index;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnChangeHealthOnServer(List<DataChange> changes)
    {
        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "curHealth":
                    short newHealth = (short)dataChanges.Value;
                    _health.ChangeHealthHandler(newHealth);
                    break;
                case "maxHealth":
                    short maxHealth = (short)dataChanges.Value;
                    _health.SetMaxHealth(maxHealth);
                    break;
                default:
                    break;
            }
        }
    }
}
