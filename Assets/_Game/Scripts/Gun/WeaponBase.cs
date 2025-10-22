using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Bullet _prefab;

    protected int MaxAmmo = 30;
    protected int SizeForCartridges = 12;
    protected int CurrentAmmo;
    protected int CurrentAmmoInCartridg;
    protected float Range;
    protected float BulletSpeed;
    protected float Spread;
    protected bool IsAutomatic;
    protected int Damage;
    protected float TimeToReloading;

    public bool IsReloadingProcces { get; protected set; }

    protected virtual void Start()
    {
        CurrentAmmo = MaxAmmo;
        CurrentAmmoInCartridg = SizeForCartridges;
    }

    public virtual void Shoot()
    {
        if (IsReloadingProcces ) return;

        if (TrySpendAmmo())
        {
            _animator.Play("Shoot");

        }
        

    }

    private protected virtual bool TrySpendAmmo()
    {
        if (CurrentAmmoInCartridg <= 0) return false;

        CurrentAmmoInCartridg--;
        CurrentAmmo--;

        if (CurrentAmmoInCartridg == 0)
        {
            Reload().Forget();
        }

        return true;
    }

    public async virtual UniTaskVoid Reload()
    {
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