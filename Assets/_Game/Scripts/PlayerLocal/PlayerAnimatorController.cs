using UnityEngine;
using UniRx;
[RequireComponent(typeof(Animator))]

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _movementModel;
    private Animator _footAnimator;
    
    private void Start()
    {
        _footAnimator = GetComponent<Animator>();

        _movementModel.IsGrounded.Subscribe(_ => SetGroundAnimator(_)).AddTo(this);
        _movementModel.Speed.Subscribe(_ => SetSpeedAnimatior(_)).AddTo(this);
        _movementModel.IsDieState.Subscribe(_ => SetIdleAnimator()).AddTo(this);
    }

    private void SetSpeedAnimatior(float speed)
    {
        speed = Mathf.Clamp(speed, -1, 1);
        _footAnimator.SetFloat("Speed", speed);
    }

    private void SetGroundAnimator(bool isGrounded)
    {
        _footAnimator.SetBool("Grounded", isGrounded);
    }

    private void SetIdleAnimator()
    {
        _footAnimator.SetTrigger("DIe");
    }
}
