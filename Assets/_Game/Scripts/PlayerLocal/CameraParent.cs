using UnityEngine;

public class CameraParent : MonoBehaviour
{
    [SerializeField] private Transform _aimTranform;

    void Start()
    {
        Transform cameraTransform = Camera.main.transform;
        cameraTransform.SetParent(this.transform);
        cameraTransform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float length = 20f; 
        Ray ray = new(transform.position, transform.forward);
        Gizmos.DrawRay(ray.origin, ray.direction * length);
    }
}
