
using UnityEngine;

public class PlayerLocal : MonoBehaviour
{
    [SerializeField] private Health _health;
    public void Init(Player player)
    {
        player.OnChange += _health.OnChangeHealthPlayrLocal;
    }  
}
