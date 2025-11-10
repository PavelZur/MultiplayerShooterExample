using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    [field: SerializeField] public Transform PointNear { get; private set; }
    [field: SerializeField] public Transform PointFar { get; private set; }
    [field: SerializeField] public float FieldView { get; private set; }
    [field: SerializeField] public float TimeToAiming { get; private set; }
    [field: SerializeField] public float OffsetOnCamera { get; private set; }
    public Transform _rootWeaponTranform { get; private set; }
    public Vector3 StartLocalPosition { get; private set; }
    public Quaternion StartLocalRotation { get; private set; }

    private void OnEnable()
    {
        _rootWeaponTranform = transform.parent;

        StartLocalPosition = _rootWeaponTranform.localPosition;
        StartLocalRotation = _rootWeaponTranform.localRotation;
    }
}
