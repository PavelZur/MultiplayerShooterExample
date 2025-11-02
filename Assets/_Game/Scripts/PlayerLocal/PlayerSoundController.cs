using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private EnemyShootingController _enemyShootingController;
    [SerializeField] private ShootingController _shootingController;
    [SerializeField] private AudioSource _playerAudioSource;
    [SerializeField] private AudioClip _shootClip;

    private void Start()
    {
        if (_shootingController != null)
        {
            _shootingController.OnShootEvent += OneShootWeapon;
        }
        if (_enemyShootingController != null)
        {
            _enemyShootingController.OnShootViewEvent += OneShootWeapon;
        }
       
    }

    private void OneShootWeapon(Vector3 any)
    {
        _playerAudioSource.PlayOneShot(_shootClip);
    }

    private void OneShootWeapon()
    {
        _playerAudioSource.PlayOneShot(_shootClip);
    }

    private void OnDestroy()
    {
        if (_shootingController != null)
        {
            _shootingController.OnShootEvent -= OneShootWeapon;
        }
        if (_enemyShootingController != null)
        {
            _enemyShootingController.OnShootViewEvent -= OneShootWeapon;
        }
    }
} 
