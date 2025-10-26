using UnityEngine;

public class RayVisualizer : MonoBehaviour
{
    [SerializeField] private Transform _bulletStartTransform;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject hitSpherePrefab;

    void LateUpdate()
    {
        Ray ray = new Ray(_bulletStartTransform.position, _bulletStartTransform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100f))
        {
            DrawLine(_bulletStartTransform.position, hit.point);
        }
        else
        {
            DrawLine(_bulletStartTransform.position, _bulletStartTransform.position + (ray.direction * 100f));
        }
    }

    private void DrawLine(Vector3 StartPos, Vector3 endPos)
    {
        _lineRenderer.SetPosition(0, StartPos);
        _lineRenderer.SetPosition(1, endPos);
        hitSpherePrefab.transform.position = endPos;
    }
}
