using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponModel _weaponModel;
    [SerializeField] private List<WeaponTypePair> _weaponTypePairs = new();
    public Dictionary<TypeWeapon, WeaponBase> AllWeaponsOfType { get; private set; } = new();
    [field: SerializeField] public WeaponBase CurrentActiveWeapon { get; private set; }
    public bool IsInit;

    private void Start()
    {
        IsInit = false;

        CreateWeaponDictionaty();
        _weaponModel.CurrentActiveWeapon.Subscribe(weapon => ChangeActiveWeapon(AllWeaponsOfType[weapon])).AddTo(this);

        IsInit = true;
    }



    public void ChangeActiveWeapon(WeaponBase weapon)
    {
        if (CurrentActiveWeapon == weapon) return;

        CurrentActiveWeapon = weapon;
        _weaponModel.CurrentActiveWeapon.Value = CurrentActiveWeapon.TypeWeapon;

        ChangeWeaponView(weapon).Forget();
    }

    public bool IsChangeWeaponProccess;

    private async UniTaskVoid ChangeWeaponView(WeaponBase weapon)
    {
        IsChangeWeaponProccess = true;

        await UniTask.WaitForSeconds(0.1f);

        foreach (var type in AllWeaponsOfType)
        {
            type.Value.Hide();
        }

        foreach (var key in AllWeaponsOfType)
        {
            if (key.Value == weapon)
            {
                key.Value.Show();
            }
        }     

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
