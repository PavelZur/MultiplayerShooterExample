using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _imageFill;
    private void OnEnable()
    {
        _health.CerrentHelth.Subscribe(cerent => UpdateViewHealth(cerent, _health.MaxHealth)).AddTo(this);
    }

    private void UpdateViewHealth(int cerent,int max)
    {
        _imageFill.fillAmount = (float)cerent / max;
    }
}
