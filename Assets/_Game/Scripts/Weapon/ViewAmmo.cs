using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ViewAmmo : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private TextMeshProUGUI _textAmmo;

    private async UniTaskVoid Start()
    {
        await UniTask.WaitUntil(() => _weaponController.IsInit);

        foreach (var key in _weaponController.AllWeaponsOfType)
        {
            key.Value.AmmoUpdateCountEvent += UpdateTextAmmo;
        }
    }

    private void UpdateTextAmmo(int inCatrid, int cerrent)
    {
        _textAmmo.text = $"{inCatrid}/{cerrent}";
    }

    private void OnDisable()
    {
        foreach (var key in _weaponController.AllWeaponsOfType)
        {
            key.Value.AmmoUpdateCountEvent -= UpdateTextAmmo;
        }
    }
}
