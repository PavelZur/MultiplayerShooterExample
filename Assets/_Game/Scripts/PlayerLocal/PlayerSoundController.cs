using UnityEngine;
using UniRx;

public class PlayerSoundController : MonoBehaviour
{
    // [SerializeField] private EnemyShootingController _enemyShootingController;

    [SerializeField] private PlayerDataReceiver _playerDataReceiver;
    [SerializeField] private ShootingController _shootingController;
    [SerializeField] private PlayerMovementModel _playerMovementModel;
    [SerializeField] private PlayerMovementController _playerMovementController;

    [SerializeField] private AudioSource _playerShootAudioSource;
    [SerializeField] private AudioSource _playerMoveAudioSource;

    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _moveClip;

    private void Start()
    {
        if (_shootingController != null)
        {
            _shootingController.OnShootEvent += OneShootWeapon;
        }

        _playerDataReceiver.PlayerDieEvent += PlayDie;

        _playerMovementModel.PlayerVelosity.Subscribe(velosity => PlaySteps(velosity.sqrMagnitude > 0.2 && _playerMovementModel.IsGrounded.Value)).AddTo(this);
        _playerMovementController.IsJumping.Where(isJump => isJump).Subscribe(isJump => PlayJump()).AddTo(this);
    }

    private void OneShootWeapon(Vector3 any)
    {
        Debug.Log("shoot");
        _playerShootAudioSource.PlayOneShot(_shootClip);
    }

    //private void OneShootWeapon()
    //{
    //    _playerShootAudioSource.PlayOneShot(_shootClip);
    //}

    private void PlayDie(bool value)
    {
        if (value)
        {
            _playerShootAudioSource.PlayOneShot(_dieClip);
        }
    }

    private void PlayJump()
    {
        _playerShootAudioSource.PlayOneShot(_jumpClip,0.1f);
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
        if (_shootingController != null)
        {
            _shootingController.OnShootEvent -= OneShootWeapon;
        }
        //if (_enemyShootingController != null)
        //{
        //    _enemyShootingController.OnShootViewEvent -= OneShootWeapon;
        //}

        _playerDataReceiver.PlayerDieEvent -= PlayDie;
    }
}
