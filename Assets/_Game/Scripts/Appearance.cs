using System.Collections;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    [SerializeField] private float _startScale = 0.5f;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private Vector3 _finalScale = Vector3.one;
    private void OnEnable()
    {
        StartCoroutine(ScaleInCoroutine());
    }

    private IEnumerator ScaleInCoroutine()
    {

        Vector3 initialScale = Vector3.one * _startScale;
        Vector3 finalScale = _finalScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float progress = elapsedTime / animationDuration;
            float curveValue = scaleCurve.Evaluate(progress);

            transform.localScale = Vector3.LerpUnclamped(initialScale, finalScale, curveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
