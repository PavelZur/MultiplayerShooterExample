using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathInfoCartPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textKiller;
    [SerializeField] private TextMeshProUGUI _textDead;
    [SerializeField] private Image _headImage;
    public void Init(string killer, string dead, bool isHead)
    {
        _textKiller.text = killer;
        _textDead.text = dead;
        _headImage.enabled = isHead;
    }
}
