using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;
using UniRx;

public class Health : MonoBehaviour
{
    [Header("IsEnemy")]
    [SerializeField] private bool _isEnemy;
    [SerializeField] private Enemy _enemy;

    [Header("All")]
    public int MaxHealth { get; private set; } = 100;
    public ReactiveProperty<int> CerrentHelth = new();

    private void Start()
    {
       // SetMaxHealth();

        if (_isEnemy)
        {
            CerrentHelth.Subscribe(_ => OnChangeHealthEnemy(_)).AddTo(this);
        }       
    }

    public void RemoveHealth(int value)
    {
        int cerent = Mathf.Clamp(CerrentHelth.Value - value, 0, MaxHealth);
        CerrentHelth.Value = cerent;
    }

    public void AddHealth(int value)
    {
        int cerent = Mathf.Clamp(CerrentHelth.Value + value, 0, MaxHealth);
        CerrentHelth.Value = cerent;
    }

    public void SetMaxHealth()
    {
        CerrentHelth.Value = MaxHealth;
    }

    public void OnChangeHealthPlayrLocal(List<DataChange> changes)
    {
        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "health":
                    float newHealth = (float)dataChanges.Value;
                    Debug.Log("HealthPlayrLocal " + newHealth);
                    if (newHealth < CerrentHelth.Value)
                    {
                        RemoveHealth(CerrentHelth.Value - (int)newHealth);
                    }
                    else if (newHealth > CerrentHelth.Value)
                    {
                        AddHealth((int)newHealth - CerrentHelth.Value);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnChangeHealthEnemy(int cerrentHealth)
    {
        Dictionary<string, object> data = new()
        {
            { "id", _enemy.SessionId },
            { "health", cerrentHealth }
        };

        Debug.Log("SendHealthEnemy" + cerrentHealth);
        MultiplayerManager.Instance.SendMessageColyseus("damage", data);
    }
}
