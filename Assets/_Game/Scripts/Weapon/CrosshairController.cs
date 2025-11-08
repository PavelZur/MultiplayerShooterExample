using UnityEngine;
using UniRx;
using System;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private ShootingController _shootingController;
    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private float _maxSpread = 55;
    [SerializeField] private float _minSpread = 20;
    [SerializeField] private float SpeedSpread = 60;
    [SerializeField] private PartsCrosshair[] _partsCrosshairArray;

    public float TargetSpread;
    private float curSpread;

    public Action BlinkEvent;

    private void OnEnable()
    {
        _shootingController.OnShootEventOnSync += OnShootHandler;
    }

    private void OnDisable()
    {
        _shootingController.OnShootEventOnSync -= OnShootHandler;
        BlinkEvent = null;
    }

    private void OnShootHandler(Vector3 any, Vector3 any2, byte type)
    {
        if (type == (byte)DecalBulletType.Blood)
        {
            BlinkEvent?.Invoke();
        }

        UpdateTargetSpreadOnShoot();
    }

    private void UpdateTargetSpreadOnMove()
    {
        float target = _minSpread + _playerMovementModel.Speed.Value;
        if (target < TargetSpread)
        {
            return;
        }
        TargetSpread = _playerMovementModel.Speed.Value > 0 ? _minSpread + _playerMovementModel.Speed.Value : TargetSpread;
    }

    private void UpdateTargetSpreadOnShoot()
    {
        float target = Mathf.Clamp(TargetSpread += 15,_minSpread,_maxSpread);
        TargetSpread = target;
    }


    private void Update()
    {
        TargetSpread =  Mathf.Clamp(TargetSpread -= SpeedSpread * Time.deltaTime, _minSpread, _maxSpread);
        UpdateTargetSpreadOnMove();
        CrosshairUpdate();
    }

    private void CrosshairUpdate()
    {
        curSpread = Mathf.Lerp(curSpread, TargetSpread, SpeedSpread * Time.deltaTime);

        foreach (var item in _partsCrosshairArray)
        {
            item.PartsRect.anchoredPosition = item.PartPos * curSpread;
        }
    }
}

[System.Serializable]
public class PartsCrosshair
{
    public RectTransform PartsRect;
    public Vector2 PartPos;
}
