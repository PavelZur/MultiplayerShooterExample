using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UniRx;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private PlayerWeaponModel _weaponModel;
    [SerializeField] private WeaponViewUI _weaponViewUI;
    [SerializeField] private List<WeaponTypePair> _weaponTypePairs = new();
    public Dictionary<TypeWeapon, WeaponBase> AllWeaponsOfType { get; private set; } = new();
    [field: SerializeField] public WeaponBase CurrentActiveWeapon { get; private set; }

    public Action OnReloadGunEvent;
    public Action OnChangeWeaponEvent;

    public bool IsInit;

    private void Start()
    {
        IsInit = false;

        CreateWeaponDictionaty();

        foreach (var weapon in AllWeaponsOfType)
        {
            weapon.Value.ReloadGun += () =>
            {
                OnReloadGunEvent?.Invoke();     
            };
        }

        foreach (var item in AllWeaponsOfType)
        {
            _weaponViewUI.Create(item.Key, ChangeActiveWeapon);
        }

        _playerMovementModel.IsDieState.Where(_ => _).Subscribe(_=> {
            foreach (var item in AllWeaponsOfType)
            {
                item.Value.ResetAmmo();
            }
        } ).AddTo(this);

        IsInit = true;
    }

    private void Update()
    {
        if (!IsInit) return;

        if (InputController.Instance.ReloadWeaponKeyPressed)
        {
            if (CurrentActiveWeapon!= null)
            {
                CurrentActiveWeapon.Reload().Forget();
            }
        }
    }

    public void ChangeActiveWeapon(TypeWeapon typeWeapon)
    {
        if (!AllWeaponsOfType.ContainsKey(typeWeapon) || CurrentActiveWeapon.TypeWeapon == typeWeapon) return;

        CurrentActiveWeapon = AllWeaponsOfType[typeWeapon];
        _weaponModel.CurrentActiveWeapon.Value = CurrentActiveWeapon.TypeWeapon;

        ChangeWeaponView(typeWeapon).Forget();
    }

    public bool IsChangeWeaponProccess;

    private async UniTaskVoid ChangeWeaponView(TypeWeapon typeWeapon)
    {
        IsChangeWeaponProccess = true;
        OnChangeWeaponEvent?.Invoke();

        await UniTask.WaitForSeconds(0.1f);

        foreach (var weapon in AllWeaponsOfType)
        {
            weapon.Value.Hide();
        }
        AllWeaponsOfType[typeWeapon].Show();

        IsChangeWeaponProccess = false;
    }

    private void CreateWeaponDictionaty()
    {
        AllWeaponsOfType.Clear();

        foreach (var pair in _weaponTypePairs)
        {
            AllWeaponsOfType.Add(pair.type, pair.weapon);
        }
    }
}

[Serializable]
public struct WeaponTypePair
{
    public TypeWeapon type;
    public WeaponBase weapon;
}
