using UnityEngine;

[CreateAssetMenu(fileName = "WeaponParam", menuName = "Parameters/WeaponParam", order = 1)]
public class WeaponParam : ScriptableObject
{
    [field: SerializeField] public BulletPool BulletPoolObj { get; private set; } 
    [field: SerializeField] public int Damage { get; private set; } = 10;
    [field: SerializeField] public int MaxAmmo { get; private set; } = 30;
    [field: SerializeField] public int SizeForCartridges { get; private set; } = 12;
    [field: SerializeField] public float TimeToReloading { get; private set; } = 1;
    [field: SerializeField] public float Range { get; private set; } = 0;
    [field: SerializeField] public float BulletSpeed { get; private set; } = 170;
    [field: SerializeField] public float PeriodShooting { get; private set; } = 0f;
    [field: SerializeField] public bool IsAutomatic { get; private set; } = false;
}
