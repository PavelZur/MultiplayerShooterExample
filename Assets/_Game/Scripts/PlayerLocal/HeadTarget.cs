using UnityEngine;

public class HeadTarget : MonoBehaviour
{
    [SerializeField] private Transform _transformFoot;

    private void Update()
    {
        _transformFoot.position = transform.position;
    }
}
