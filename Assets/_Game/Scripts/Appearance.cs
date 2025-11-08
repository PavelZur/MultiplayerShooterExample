using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    [SerializeField] private float _startScale = 0.5f;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private Vector3 _finalScale = Vector3.one;
    private CancellationTokenSource _cts;

    private async UniTaskVoid ScaleInAsync(CancellationToken token)
    {
        Vector3 initialScale = Vector3.one * _startScale;
        Vector3 finalScale = _finalScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            if (token.IsCancellationRequested)
                return;

            float progress = elapsedTime / animationDuration;
            float curveValue = scaleCurve.Evaluate(progress);

            transform.localScale = Vector3.LerpUnclamped(initialScale, finalScale, curveValue);

            await UniTask.Yield(PlayerLoopTiming.Update, token);
            elapsedTime += Time.deltaTime;
        }

        transform.localScale = finalScale;
    }

    private void OnEnable()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        ScaleInAsync(_cts.Token).Forget();
    }

    private void OnDisable()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }
}
