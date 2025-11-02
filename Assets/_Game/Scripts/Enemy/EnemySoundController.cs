using UniRx;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    [SerializeField] private EnemyShootingController _enemyShootingController;
    [SerializeField] private EnemyDataReceiver _playerDataReceiver;
    [SerializeField] private PlayerMovementModel _playerMovementModel;

    [SerializeField] private AudioSource _playerShootAudioSource;
    [SerializeField] private AudioSource _playerMoveAudioSource;

    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField] private AudioClip _moveClip;

    private void Start()
    {
        
        if (_enemyShootingController != null)
        {
            _enemyShootingController.OnShootViewEvent += OneShootWeapon;
        }

        _playerDataReceiver.Die += PlayDie;

        _playerMovementModel.PlayerVelosity.Subscribe(velosity => PlaySteps(velosity.sqrMagnitude > 0.2 && _playerMovementModel.IsGrounded.Value)).AddTo(this);
    }
    private void OneShootWeapon()
    {
        _playerShootAudioSource.PlayOneShot(_shootClip);
    }

    private void PlayDie(bool value)
    {
        if (value)
        {
            _playerShootAudioSource.PlayOneShot(_dieClip);
        }
    }

    private void PlaySteps(bool value)
    {
        if (value)
        {
            if (!_playerMoveAudioSource.isPlaying)
            {
                _playerMoveAudioSource.volume = 0.2f;
                _playerMoveAudioSource.Play();
            }
        }
        else
        {
            if (_playerMoveAudioSource.isPlaying)
                _playerMoveAudioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        if (_enemyShootingController != null)
        {
            _enemyShootingController.OnShootViewEvent -= OneShootWeapon;
        }

        _playerDataReceiver.Die -= PlayDie;
    }
}
