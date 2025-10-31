using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiver : MonoBehaviour
{
    [SerializeField] private Health _health;

    public void InitDataReceiver(Player player)
    {
        player.healthData.OnChange += OnChangeHealthOnServer;
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
