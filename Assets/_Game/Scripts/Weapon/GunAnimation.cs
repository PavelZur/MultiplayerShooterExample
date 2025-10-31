using UniRx;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [Header("PlayerLocal")]
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private ShootingController _shootingController;

    [Header("Enemy")]
    [SerializeField] private bool _isEnemy;
    [SerializeField] private PlayerWeaponModel _playerWeaponModel;
    [SerializeField] private EnemyShootingController _enemyShootingController;

    [SerializeField] private Animator _animator;
    private const string NAME_SHOOT_ANIM = "Shoot";
    private const string NAME_RELOAD_ANIM = "ReloadGun";

    private void OnEnable()
    {
        if (_isEnemy) 
        {
            _playerWeaponModel.CurrentActiveWeapon.Skip(1).Subscribe(_ => PlayChangeAnim()).AddTo(this);
            MultiplayerManager.Instance.OnEnemyGunReloadEvent += PlayReloadAnim;
            _enemyShootingController.OnShootEvent += PlayShootAnim;
        }
        else
        {
            _weaponController.OnReloadGunEvent += PlayReloadAnim;
            _weaponController.OnChangeWeaponEvent += PlayChangeAnim;
            _shootingController.OnShootEvent += PlayShootAnim;
        }      
    }

    public void PlayShootAnim(Vector3 any = default)
    {
        _animator.Play(NAME_SHOOT_ANIM);
    }

    public void PlayShootAnim()
    {
        _animator.Play(NAME_SHOOT_ANIM);
    }

    public void PlayReloadAnim()
    {
        _animator.SetTrigger("Reload");
    }

    public void PlayChangeAnim()
    {
        _animator.SetTrigger("Change");
    }

    private void OnDisable()
    {
        if (_isEnemy)
        {
            MultiplayerManager.Instance.OnEnemyGunReloadEvent -= PlayReloadAnim;
            _enemyShootingController.OnShootEvent -= PlayShootAnim;
        }
        else
        {
            _weaponController.OnChangeWeaponEvent -= PlayChangeAnim;
            _weaponController.OnReloadGunEvent -= PlayReloadAnim;
            _shootingController.OnShootEvent -= PlayShootAnim;
        }
    }
}

