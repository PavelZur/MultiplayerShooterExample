using UnityEngine;
using UniRx;

public class PlayerMovementModel : MonoBehaviour
{
    private void Awake()
    {
        PlayerPosition.Value = transform.position;
    }

    public ReactiveProperty<Vector3> PlayerPosition = new();
    public ReactiveProperty<Vector3> PlayerVelosity = new();
    public ReactiveProperty<float> PlayerRotationY = new();
    public ReactiveProperty<float> HandRotationX = new();
    public ReactiveProperty<float> Speed = new();
    public ReactiveProperty<bool> IsGrounded = new(true);
    public ReactiveProperty<bool> IsSitting = new(false);
}
