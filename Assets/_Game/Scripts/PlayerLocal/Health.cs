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
    public string SessionID { get => _enemy.SessionId; }

    public void Init(int max,int cur)
    {
        SetMaxHealth(max);
        ChangeHealthHandler(cur);
    }

    public void ChangeHealthHandler(int newHealthValue)
    {
        newHealthValue = Mathf.Clamp(newHealthValue, 0, MaxHealth);
        CerrentHelth.Value = newHealthValue;
    }

    public void SetMaxHealth(int maxHealth)
    {
        maxHealth = Mathf.Clamp(maxHealth, 0, int.MaxValue);
        MaxHealth = maxHealth;
    }
}
