using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponViewUI : MonoBehaviour
{
    [SerializeField] private Transform _horGroupe;
    [SerializeField] private WeaponSelectorUI _selectorUIPrefab;

    public void Create(TypeWeapon type, Action<TypeWeapon> changeWeapon)
    {
        WeaponSelectorUI newSlector = Instantiate(_selectorUIPrefab, _horGroupe);
        newSlector.Init(type, changeWeapon);
    }
}
