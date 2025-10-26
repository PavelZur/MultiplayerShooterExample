using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public GunAnimation GunAnimation { get; private set; }
    [field: SerializeField] public Transform BulletStartTranform { get; private set; }
    [field: SerializeField] public Bullet PrefabBullet { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int MaxAmmo { get; private set; } = 30;
    [field: SerializeField] public int SizeForCartridges { get; private set; } = 12;
    [field: SerializeField] public float TimeToReloading { get; private set; } = 1;
    [field: SerializeField] public float Range { get; private set; }
    [field: SerializeField] public float BulletSpeed { get; private set; }
    [field: SerializeField] public bool IsAutomatic { get; private set; }

    protected int CurrentAmmo;
    protected int CurrentAmmoInCartridg;

    public bool IsReloadingProcces { get; protected set; }

    public Action<int, int> AmmoUpdateCountEvent;
    public Action ReloadGun;

    protected virtual void Start()
    {
        CurrentAmmo = MaxAmmo;
        CurrentAmmoInCartridg = SizeForCartridges;
        CurrentAmmo -= CurrentAmmoInCartridg;
        AmmoUpdateCountEvent?.Invoke(CurrentAmmoInCartridg, CurrentAmmo);
    }

    public virtual bool TryShoot()
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
    
    protected async virtual UniTaskVoid Reload()
    {
        if (IsReloadingProcces) return;

        // перезарядку пока не передаю на сервер, надо както красиво обыграть у енеми то а от сюда вынести.
        ReloadGun?.Invoke();
        IsReloadingProcces = true;
        int neededAmmo = SizeForCartridges - CurrentAmmoInCartridg;

        await UniTask.WaitForSeconds(TimeToReloading);

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

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}