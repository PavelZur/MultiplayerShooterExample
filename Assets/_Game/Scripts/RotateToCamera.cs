using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    private Transform _thisTransform;
    private Transform _cameratransform;
    void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _cameratransform = Camera.main.transform;
    }

    void Update()
    {
        _thisTransform.LookAt(_cameratransform);
    }
}
