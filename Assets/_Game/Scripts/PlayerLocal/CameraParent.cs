using UnityEngine;

public class CameraParent : MonoBehaviour
{
    void Start()
    {
        Transform cameraTransform = Camera.main.transform;
        cameraTransform.SetParent(this.transform);
        cameraTransform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
