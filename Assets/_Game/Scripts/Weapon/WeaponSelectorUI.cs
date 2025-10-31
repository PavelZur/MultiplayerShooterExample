using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectorUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameWeapon;
    [SerializeField] private TextMeshProUGUI _namberKeyCodeText;
    private TypeWeapon _typeWeapon;

    KeyCode _keyCodeButton = KeyCode.None;

    public void Init(TypeWeapon typeWeapon, Action<TypeWeapon> action)
    {
        _typeWeapon = typeWeapon;
        _nameWeapon.text = typeWeapon.ToString();
        _button.onClick.AddListener(() => action?.Invoke(_typeWeapon));
        SetKeyCode(transform.GetSiblingIndex());
    }
        

    private void Update()
    {
        if (Input.GetKeyDown(_keyCodeButton))
        {
            _button.onClick?.Invoke();
        }
    }

    private void SetKeyCode(int index)
    {
        switch (index)
        {
            case 0:
                _keyCodeButton = KeyCode.Alpha1;
                _namberKeyCodeText.text = $"1";
                break;
            case 1:
                _keyCodeButton = KeyCode.Alpha2;
                _namberKeyCodeText.text = $"2";
                break;
            default:
                _keyCodeButton = KeyCode.None;
                _namberKeyCodeText.text = $"error";
                break;
        }
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

}
