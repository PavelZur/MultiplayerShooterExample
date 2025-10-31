using UniRx;
using UnityEngine;

public class PlayerWeaponModel : MonoBehaviour
{
    public ReactiveProperty<TypeWeapon> CurrentActiveWeapon  = new();
}
