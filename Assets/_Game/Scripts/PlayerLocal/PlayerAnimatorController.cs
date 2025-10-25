using UnityEngine;
using UniRx;
[RequireComponent(typeof(Animator))]

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _movementModel;
    private Animator _playerAnimator;
    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();

        _movementModel.IsGrounded.Subscribe(_ => SetGroundAnimator(_)).AddTo(this);
        _movementModel.Speed.Subscribe(_ => SetSpeedAnimatior(_)).AddTo(this);
    }

    private void SetSpeedAnimatior(float speed)
    {
        speed = Mathf.Clamp(speed, -1, 1);
        _playerAnimator.SetFloat("Speed", speed);
    }

    private void SetGroundAnimator(bool isGrounded)
    {
        _playerAnimator.SetBool("Grounded", isGrounded);
    }
}
