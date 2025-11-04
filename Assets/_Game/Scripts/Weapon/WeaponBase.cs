using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public abstract class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public Transform BulletStartTranform { get; private set; }
    [field: SerializeField] public WeaponParam WeaponParametrs { get; private set; }
    [field: SerializeField] public TypeWeapon TypeWeapon { get; private set; }

    protected int CurrentAmmo;
    protected int CurrentAmmoInCartridg;

    public bool IsReloadingProcces { get; protected set; }

    public Action<int, int> AmmoUpdateCountEvent;
    public Action ReloadGun;

    public BulletPool BulletPoolPrefabs { get; private set; }

    protected virtual void Start()
    {
        BulletPoolPrefabs = Instantiate(WeaponParametrs.BulletPoolObj,transform);

        CurrentAmmo = WeaponParametrs.MaxAmmo;
        CurrentAmmoInCartridg = WeaponParametrs.SizeForCartridges;
        CurrentAmmo -= CurrentAmmoInCartridg;
        AmmoUpdateCountEvent?.Invoke(CurrentAmmoInCartridg, CurrentAmmo);
    }

    public virtual bool TryAmmo()
    {
        if (IsReloadingProcces) return false;

        if (TrySpendAmmo())
        {
            return true;
        }

        return false;
    }

    private protected virtual bool TrySpendAmmo()
    {
        if (CurrentAmmoInCartridg <= 0)
        {
            Reload().Forget();
            return false;
        }

        CurrentAmmoInCartridg--;

        AmmoUpdateCountEvent?.Invoke(CurrentAmmoInCartridg, CurrentAmmo);

        if (CurrentAmmoInCartridg == 0)
        {
            Reload().Forget();
        }

        return true;
    }
    
    public async virtual UniTaskVoid Reload()
    {
        if (IsReloadingProcces || CurrentAmmo <= 0 || WeaponParametrs.SizeForCartridges == CurrentAmmoInCartridg) return;

        ReloadGun?.Invoke();
        IsReloadingProcces = true;
        int sizeForCartridges =  WeaponParametrs.SizeForCartridges;
        int neededAmmo = sizeForCartridges - CurrentAmmoInCartridg;

        await UniTask.WaitForSeconds(WeaponParametrs.TimeToReloading);

        if (CurrentAmmo >= neededAmmo)
        {
            CurrentAmmoInCartridg += neededAmmo;
            CurrentAmmo -= neededAmmo;
        }
        else
        {
            CurrentAmmoInCartridg += CurrentAmmo;
            CurrentAmmo = 0;
        }

        AmmoUpdateCountEvent?.Invoke(CurrentAmmoInCartridg, CurrentAmmo);
        IsReloadingProcces = false;
    }

    //public void ForceUpdateAmmo()
    //{
    //    AmmoUpdateCountEvent?.Invoke(CurrentAmmoInCartridg, CurrentAmmo);
    //}

    public virtual void Show()
    {
        gameObject.SetActive(true);
        AmmoUpdateCountEvent?.Invoke(CurrentAmmoInCartridg, CurrentAmmo);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}

[Serializable]
public enum TypeWeapon
{
    Pistol,
    Automat
}