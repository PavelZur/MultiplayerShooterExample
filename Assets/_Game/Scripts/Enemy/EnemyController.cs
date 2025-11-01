using System;
using System.Collections.Generic;
using System.Threading;
using Colyseus.Schema;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _movementModel;
    private readonly List<float> _receiveTimeInteval = new() { 0, 0, 0, 0, 0 };

    public bool IsInit;

    private readonly CancellationTokenSource _cts = new();

    public void Init(Vector3 position, Vector3 velosity, float rotationY, float rotationHand)
    {
        IsInit = false;
        _movementModel.PlayerPosition.Value = position;
        _movementModel.PlayerVelosity.Value = velosity;
        _movementModel.HandRotationX.Value = rotationHand;
        _movementModel.PlayerRotationY.Value = rotationY;
        IsInit = true;

        UpdateAverageRtt().Forget();
    }

    private async UniTaskVoid UpdateAverageRtt()
    {
        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                _receiveTimeInteval.Add((MultiplayerManager.Instance.RTT / 2) / 1000);

                if (_receiveTimeInteval.Count > 5)
                    _receiveTimeInteval.RemoveAt(0);

                await UniTask.Delay(1000, cancellationToken: _cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            
        }
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public float AvarageInterval
    {
        get
        {
            if (_receiveTimeInteval.Count == 0)
                return 0;

            float sum = 0f;
            foreach (var v in _receiveTimeInteval)
                sum += v;

            return sum / _receiveTimeInteval.Count;
        }
    }

    public void OnChangeHandler(List<DataChange> changes)
    {
        Vector3 newPos = _movementModel.PlayerPosition.Value;
        Vector3 newVelosity = _movementModel.PlayerVelosity.Value;

        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "px":
                    newPos.x = (float)dataChanges.Value;
                    break;
                case "py":
                    newPos.y = (float)dataChanges.Value;
                    break;
                case "pz":
                    newPos.z = (float)dataChanges.Value;
                    break;
                case "vx":
                    newVelosity.x = (float)dataChanges.Value;
                    break;
                case "vy":
                    newVelosity.y = (float)dataChanges.Value;
                    break;
                case "vz":
                    newVelosity.z = (float)dataChanges.Value;
                    break;
                case "ry":
                    _movementModel.PlayerRotationY.Value = (float)dataChanges.Value;
                    break;
                case "rx":
                    _movementModel.HandRotationX.Value = (float)dataChanges.Value;
                    break;
                default:
                    break;
            }
        }
        _movementModel.PlayerPosition.Value = newPos;
        _movementModel.PlayerVelosity.Value = newVelosity;
    }
}
