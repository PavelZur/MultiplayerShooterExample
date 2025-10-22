using TMPro;
using UnityEngine;

public class RTTView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textRtt;

    private void Update()
    {
        _textRtt.text = $"RTT : {MultiplayerManager.Instance.RTT} мс";
    }
}
