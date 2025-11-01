
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerLocal : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private PlayerDataReceiver _playerDataReceiver;
    [SerializeField] private WeaponController _weaponController;

    public async UniTaskVoid Init(Player player)
    {
        _health.Init(player.healthData.maxHealth, player.healthData.curHealth);
        _playerDataReceiver.InitDataReceiver(player);

        await UniTask.WaitUntil(() => _weaponController.IsInit);

        _weaponController.ChangeActiveWeapon((TypeWeapon)player.weaponData.weapon);
    }
}
