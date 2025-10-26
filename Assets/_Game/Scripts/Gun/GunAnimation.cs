using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private const string NAME_SHOOT_ANIM = "Shoot";
    private const string NAME_RELOAD_ANIM = "ReloadGun";

    public void PlayShootAnim()
    {
        _animator.Play(NAME_SHOOT_ANIM);
    }

    public void PlayReloadAnim()
    {
        Debug.Log(NAME_RELOAD_ANIM);
        _animator.SetTrigger("Reload");
    }
}
