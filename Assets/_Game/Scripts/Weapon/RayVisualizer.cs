using UnityEngine;

public class RayVisualizer : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject hitSpherePrefab;


    void LateUpdate()
    {
        if (_weaponController.CurrentActiveWeapon != null)
        {
            Ray ray = new Ray(_weaponController.CurrentActiveWeapon.BulletStartTranform.position, _weaponController.CurrentActiveWeapon.BulletStartTranform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                DrawLine(_weaponController.CurrentActiveWeapon.BulletStartTranform.position, hit.point);
            }
            else
            {
                DrawLine(_weaponController.CurrentActiveWeapon.BulletStartTranform.position, _weaponController.CurrentActiveWeapon.BulletStartTranform.position + (ray.direction * 100f));
            }
        }   
    }

    private void DrawLine(Vector3 StartPos, Vector3 endPos)
    {
        _lineRenderer.SetPosition(0, StartPos);
        _lineRenderer.SetPosition(1, endPos);
        hitSpherePrefab.transform.position = endPos;
    }
}
