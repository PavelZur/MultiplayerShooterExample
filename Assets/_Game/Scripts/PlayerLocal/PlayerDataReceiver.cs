using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiver : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private Health _health;

    public Action <bool>Die; 
    public void InitDataReceiver(Player player)
    {
        player.healthData.OnChange += OnChangeHealthOnServer;
        player.playerStatedata.OnChange += OnChangePlayerStateOnServer;
    }

    private void OnChangePlayerStateOnServer(List<DataChange> changes)
    {
        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "die":
                    bool isDieState = (bool)dataChanges.Value;

                    if (isDieState)
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                    else
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                    
                    _playerMovementModel.IsDieState.Value = isDieState;
                    Die?.Invoke(isDieState);                    
                    break;
                default:
                    break;
            }
        }
    }

    private void OnChangeHealthOnServer(List<DataChange> changes)
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
