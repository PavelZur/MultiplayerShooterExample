using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private InputController _inputController;

    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private Transform _handTranform;
    [SerializeField] private Transform _groungShperTranform;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private float _mouseSensitivity = 10;
    [SerializeField] private int _minHandAngle = -85;
    [SerializeField] private int _maxHandAngle = 85;
    [SerializeField] private float _speedMove = 8;

    [SerializeField] private float gravity = -25f;
    [SerializeField] private float jumpInitialMomentum = 8f;
    [SerializeField] private float momentumFalloffRate = 20f;

    [SerializeField] private MouseSensitivityController _mouseSensitivityController;
    [SerializeField] private LayerMask _groundMask;

    private float _cerrentHandRotateX;
    private Vector3 _velosity;

    private float _inputH;
    private float _inputV;
    private float _mouseX;
    private float _mouseY;

    private float _rotateY;

    private float _currentJumpMomentum;
    public ReactiveProperty<bool> IsJumping { get; } = new();
    private bool IsGrounded
    {
        get
        {
            return Physics.CheckSphere(transform.position, 0.15f, _groundMask);
        }
    }

    private void Start()
    {
        _inputController = InputController.Instance;
        _cerrentHandRotateX = 0;

        _mouseSensitivityController.MouseSensitivity.Subscribe(sensitivity =>  _mouseSensitivity = sensitivity).AddTo(this);
    }

    private void Update()
    {
        if (_playerMovementModel.IsDieState.Value) return;

        _inputH = _inputController.AxisRawHorizontal;
        _inputV = _inputController.AxisRawVertical;
        _mouseX = _inputController.MouseAxisX;
        _mouseY = _inputController.MouseAxisY;

        _rotateY += _mouseX;

        if (_inputController.JumpKeyPressed)
        {
            Jump().Forget();
        }

        RotateHand();
        _playerMovementModel.IsSitting.Value = InputController.Instance.SittingKeyPressed;
    }

    private void FixedUpdate()
    {
        if (!_playerMovementModel.IsDieState.Value)
        {
            RotateY();
            ApplyGravity();
            Move();
            SetValuesOnPlayerModel();
        }       
    }

    public void TranslatePlayerOnRestart(Vector3 pointPos)
    {
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.None;
        _rb.linearVelocity = Vector3.zero;

        _rb.position = pointPos;

        _rb.isKinematic = false;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Move()
    {
        Vector3 moveMomentum = ((transform.forward * _inputV + transform.right * _inputH).normalized) * _speedMove;
        _velosity = new(moveMomentum.x, moveMomentum.y + _currentJumpMomentum, moveMomentum.z);
        _rb.linearVelocity = _velosity;
    }

    private void RotateY()
    {
        _rb.angularVelocity = new(0f, _rotateY * _mouseSensitivity, 0f);
        _rotateY = 0;
    }

    private void RotateHand()
    {
        float x = _mouseY * -_mouseSensitivity;
        _cerrentHandRotateX = Mathf.Clamp(_cerrentHandRotateX + x, _minHandAngle, _maxHandAngle);
        _handTranform.localEulerAngles = new(_cerrentHandRotateX, 0, 0);
    }

    private async UniTaskVoid Jump()
    {
        if (IsGrounded)
        {
            IsJumping.Value = true;
            _currentJumpMomentum = jumpInitialMomentum;
            await UniTask.WaitForSeconds(0.5f);
            IsJumping.Value = false;
        }
    }

    private void ApplyGravity()
    {
        if (!IsJumping.Value && IsGrounded)
        {
            _currentJumpMomentum = 0;
            return;
        }

        if (_currentJumpMomentum > gravity)
        {
            _currentJumpMomentum -= momentumFalloffRate * Time.fixedDeltaTime;
            if (_currentJumpMomentum < gravity)
                _currentJumpMomentum = gravity;
        }
    }

    private void SetValuesOnPlayerModel()
    {
        _playerMovementModel.PlayerPosition.Value = _rb.position;
        _playerMovementModel.PlayerVelosity.Value = _rb.linearVelocity;
        _playerMovementModel.PlayerRotationY.Value = _rb.rotation.eulerAngles.y;
        _playerMovementModel.HandRotationX.Value = _cerrentHandRotateX;
        _playerMovementModel.Speed.Value = GetSpeed();
        _playerMovementModel.IsGrounded.Value = IsGrounded;
    }

    private float GetSpeed()
    {
        if (_rb.linearVelocity.magnitude < 0.2)
        {
            return 0;
        }
        else
        {
            if (Vector3.Dot(_rb.linearVelocity.normalized, transform.forward) < 0)
            {
                return _rb.linearVelocity.magnitude * -1;
            }
        }
        return _rb.linearVelocity.magnitude;
    }   
}
