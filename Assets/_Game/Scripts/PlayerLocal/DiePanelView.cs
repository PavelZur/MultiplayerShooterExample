using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

public class DiePanelView : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _movementModel;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _buttonRestart;
    public Action OnButtonRestartPressedEvent;
    void Start()
    {
        _movementModel.IsDieState.Subscribe(isDie => PanelVisibleHandler(isDie)).AddTo(this);
        _buttonRestart.onClick.AddListener(() => OnButtonRestartPressedEvent?.Invoke());
        _buttonRestart.onClick.AddListener(() => PanelVisibleHandler(false));
    }

    private void PanelVisibleHandler(bool value)
    {
        if (value)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        _panel.SetActive(true);
    }

    private void Hide()
    {
        _panel.SetActive(false);
    }

    private void OnDestroy()
    {
        _buttonRestart.onClick.RemoveAllListeners();
    }
}
