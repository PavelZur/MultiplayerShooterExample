using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private PlayerMovementModel _movementModel;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _handTranform;
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 _targetPosition;
    private float _velosityMagnitude;
    private bool IsGrounded
    {
        get
        {
            return Physics.CheckSphere(transform.position, 0.2f, _groundMask);
        }
    }


    private void Update()
    {
        if (!_enemyController.IsInit) return;
        RotateHand();
    }

    private void FixedUpdate()
    {
        if (!_enemyController.IsInit) return;

        _movementModel.IsGrounded.Value = IsGrounded;
        _movementModel.Speed.Value = GetSpeed();

        SetPosition(_movementModel.PlayerPosition.Value, _movementModel.PlayerVelosity.Value, _enemyController.AvarageInterval);

        if (_velosityMagnitude > 0.2f)
        {
            float maxDistance = _velosityMagnitude * Time.fixedDeltaTime;
            Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);

            _rigidbody.MovePosition(newPosition);
        }
        else
        {
            _rigidbody.MovePosition(_targetPosition);
        }

        RotateY();
    }

    private void SetPosition(Vector3 pos, Vector3 velosity, float averageInterval)
    {
        _targetPosition = pos + (velosity * averageInterval);
        _velosityMagnitude = velosity.magnitude;
    }

    private void RotateY()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, _movementModel.PlayerRotationY.Value, 0f);
        _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, 30f * Time.fixedDeltaTime));
    }

    private void RotateHand()
    {
        Quaternion targetRotation = Quaternion.Euler(_movementModel.HandRotationX.Value, 0f, 0f);
        _handTranform.localRotation = Quaternion.Lerp(_handTranform.localRotation, targetRotation, 30f * Time.deltaTime);
    }

    private float GetSpeed()
    {
        if (_velosityMagnitude < 0.2)
        {
            return 0;
        }
        else
        {
            if (Vector3.Dot(_movementModel.PlayerVelosity.Value.normalized, transform.forward) < 0)
            {
                return _velosityMagnitude * -1;
            }
        }
        return _velosityMagnitude;
    }
}
