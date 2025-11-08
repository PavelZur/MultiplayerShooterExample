using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathInfoCartPrefab : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _textKiller;
    [SerializeField] private TextMeshProUGUI _textDead;
    [SerializeField] private Image _headImage;
    public void Init(string killer, string dead, bool isHead)
    {
        _textKiller.text = killer;
        _textDead.text = dead;
        _headImage.enabled = isHead;

        DelayAlphaDestroy().Forget();
    }

    private async UniTaskVoid DelayAlphaDestroy()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        await UniTask.Delay(3000);

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = 1 - (elapsed / duration);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        _canvasGroup.alpha = 0f;

        Destroy(gameObject);
    }
}
