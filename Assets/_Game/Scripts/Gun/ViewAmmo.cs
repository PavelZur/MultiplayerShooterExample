using TMPro;
using UnityEngine;

public class ViewAmmo : MonoBehaviour
{
    [SerializeField] private WeaponBase _weaponBase;
    [SerializeField] private TextMeshProUGUI _textAmmo;
    private void OnEnable()
    {
        _weaponBase.AmmoUpdateCountEvent += UpdateTextAmmo;
    }

    private void UpdateTextAmmo(int inCatrid, int cerrent)
    {
        _textAmmo.text = $"{inCatrid}/{cerrent}";
    }

    private void OnDisable()
    {
        _weaponBase.AmmoUpdateCountEvent -= UpdateTextAmmo;
    }
}
