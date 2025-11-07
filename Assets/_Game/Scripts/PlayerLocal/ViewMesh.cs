using UnityEngine;
using UniRx;

public class ViewMesh : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private GameObject _mesh;
    [SerializeField] private DieEffect _dieEffect;
    [SerializeField] private bool _isEnemy;
    private void Start()
    {
        _playerMovementModel.IsDieState.Subscribe(_=> ViewMeshHandler(_)).AddTo(this);

        if (_isEnemy)
        {
            _playerMovementModel.IsDieState.Where(_ => _).Subscribe(_ => EnableDieEffect()).AddTo(this);
        }  
    }

    private void EnableDieEffect()
    {
        Instantiate(_dieEffect,transform.position,Quaternion.identity);
    }

    private void ViewMeshHandler(bool value)
    {
        _mesh.SetActive(!value);
    }
}
