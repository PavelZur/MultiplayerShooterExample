using UniRx;
using UnityEngine;

public class SittingState : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private Transform _scaleTransform;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    private float _sittingScaleFactor = 0.5f;
    private float _standartScaleZ;
    private float _standartHeight;


    private void Awake()
    {
        _standartScaleZ = _scaleTransform.localScale.z;
        _standartHeight = _capsuleCollider.height;
    }

    private void Start()
    {
        _playerMovementModel.IsSitting.Skip(1).Where(_ => _ == true).Subscribe(_ => Sitting()).AddTo(this);
        _playerMovementModel.IsSitting.Skip(1).Where(_ => _ == false).Subscribe(_ => GetUp()).AddTo(this);
    }

    private void Sitting()
    {
        _scaleTransform.localScale = new (_scaleTransform.localScale.x, _scaleTransform.localScale.y, _scaleTransform.localScale.z * _sittingScaleFactor);
        _capsuleCollider.height *= _sittingScaleFactor;
    }

    private void GetUp()
    {
        _scaleTransform.localScale = new(_scaleTransform.localScale.x, _scaleTransform.localScale.y, _standartScaleZ);
        _capsuleCollider.height = _standartHeight;
    }
}
