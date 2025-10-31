using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private EnemyController _controller;

    private Player _player;
    public string SessionId { get; private set; }

    public void Init(Player player,string sessionId)
    {
        SessionId = sessionId;
        _player = player;
        _health.CerrentHelth.Value = (int)player.healthData.curHealth;
        _player.OnChange += _controller.OnChangeHandler;
    }

    public void Kill()
    {
        //пока тут реализация супер простая , смерти как таковой нет , тупо дестрой.
        _player.OnChange -= _controller.OnChangeHandler;
        Destroy(gameObject);
    }
}
