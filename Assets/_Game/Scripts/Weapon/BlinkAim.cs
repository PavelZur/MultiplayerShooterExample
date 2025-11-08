using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class BlinkAim : MonoBehaviour
{
    [SerializeField] private GameObject _imageAim;
    [SerializeField] private CrosshairController _crosshairController;

    private CancellationTokenSource _cts;

    private void OnEnable()
    {

        _crosshairController.BlinkEvent += () => Blink().Forget();
    }

    public async UniTaskVoid Blink()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            _imageAim.SetActive(true);
            await UniTask.Delay(100, cancellationToken: _cts.Token);
            _imageAim.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            
        }
    }
}
