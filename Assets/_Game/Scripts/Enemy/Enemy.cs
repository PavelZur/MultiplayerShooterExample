using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        _player.OnChange += _controller.OnChangeHandler;
    }

    public void Kill()
    {
        //пока тут реализация супер простая , смерти как таковой нет , тупо дестрой.
        _player.OnChange -= _controller.OnChangeHandler;
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        Debug.Log($"TakeDamage  {_player.__refId}" );
    }
}
