using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public ReactiveProperty<float> MouseSensitivity = new(3f);  
    void Start()
    {
        sensitivitySlider.minValue = 1f;
        sensitivitySlider.maxValue = 10f;
        sensitivitySlider.value = MouseSensitivity.Value;
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    void OnSensitivityChanged(float value)
    {
        MouseSensitivity.Value = value;
    }
}
