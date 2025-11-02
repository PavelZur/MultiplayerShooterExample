using UnityEngine;
using UniRx;

public class ViewMesh : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private GameObject _mesh;

    private void Start()
    {
        _playerMovementModel.IsDieState.Subscribe(_=> ViewMeshHandler(_)).AddTo(this);
    }

    private void ViewMeshHandler(bool value)
    {
        _mesh.SetActive(!value);
    }
}
