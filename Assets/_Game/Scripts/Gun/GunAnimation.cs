using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private bool _isEnemy;
    [SerializeField] private Animator _animator;
    private const string NAME_SHOOT_ANIM = "Shoot";
    private const string NAME_RELOAD_ANIM = "ReloadGun";

    private void OnEnable()
    {
        if (!_isEnemy) return;

        MultiplayerManager.Instance.OnEnemyGunReloadEvent += PlayReloadAnim;
    }

    public void PlayShootAnim()
    {
        _animator.Play(NAME_SHOOT_ANIM);
    }

    public void PlayReloadAnim()
    {
        Debug.Log(NAME_RELOAD_ANIM);
        _animator.SetTrigger("Reload");
    }

    private void OnDisable()
    {
        if (!_isEnemy) return;
        MultiplayerManager.Instance.OnEnemyGunReloadEvent -= PlayReloadAnim;
    }
}

