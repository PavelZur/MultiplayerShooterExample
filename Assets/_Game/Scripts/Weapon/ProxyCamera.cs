using UnityEngine;

public class ProxyCamera : MonoBehaviour
{
    private Transform _transformCamera;
    void Start()
    {
        _transformCamera = Camera.main.transform;
    }

    void Update()
    {
        transform.SetPositionAndRotation(_transformCamera.position, _transformCamera.rotation);
    }
}
