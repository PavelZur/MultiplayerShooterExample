using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _movementModel;
    private List<float> _receiveTimeInteval = new() { 0, 0, 0, 0, 0 };
    private float _lastReceiveTime = 0f;

    public float AvarageInterval
    {
        get
        {
            int count = _receiveTimeInteval.Count;
            float summ = 0;

            for (int i = 0; i < count; i++)
            {
                summ += _receiveTimeInteval[i];
            }

            return (summ / count)/2;
        }
    }

    private void SaveReceive()
    {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _receiveTimeInteval.Add(interval);
        _receiveTimeInteval.RemoveAt(0);
    }

    // тут вообще хотелосьбы какуюто надстройку что бы расрепределить сначала тип изменений по моделям или сервисам которые уже ти изменения будут обрабатывать,
    // я незнаю как это будет в дальнешем, пока пускай так, но очевидно что если у нас будет 40 полей зменений то
    // тут обрабатываться напрямую конечно не должны
    public void OnChangeHandler(List<DataChange> changes)
    {
        SaveReceive();

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
                      Debug.Log(" Пришло - vy  = " + (float)dataChanges.Value);
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
                case "ground":
                    _movementModel.IsGrounded.Value = (float)dataChanges.Value == 1 ? true : false;
                    break;
                case "anspeed":
                    _movementModel.Speed.Value = (float)dataChanges.Value;
                    break;
                case "sit":
                    _movementModel.IsSitting.Value = (float)dataChanges.Value == 1 ? true : false;
                    break;

                default:
                    Debug.LogWarning($"{name} : не обрабатывается поле {dataChanges.Field}");
                    break;
            }
        }
        _movementModel.PlayerPosition.Value = newPos;
        _movementModel.PlayerVelosity.Value = newVelosity;
    }
}
