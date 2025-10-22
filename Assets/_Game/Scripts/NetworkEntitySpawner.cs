using System.Collections.Generic;
using UnityEngine;

public class NetworkEntitySpawner : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _characterPrefab;
    [SerializeField] private Enemy _enemyPrefab;

    private Dictionary<int,Enemy> _playersOnRoom = new();

    void Start()
    {
        MultiplayerManager.Instance.OnCreatePlayerLocal += CreatePlayer;
        MultiplayerManager.Instance.OnCreateEnemy += CreateEnemy;
        MultiplayerManager.Instance.OnRemoveEnemy += RemoveEnemy;
    }

    private void CreateEnemy(Player enemyDataRemote)
    {
        Vector3 position = new(enemyDataRemote.px, enemyDataRemote.py, enemyDataRemote.pz);
        Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.Init(enemyDataRemote);
        _playersOnRoom.Add(enemyDataRemote.__refId, enemy);
    }

    private void CreatePlayer(Player playerDataRemote)
    {
        Vector3 position = new(playerDataRemote.px, playerDataRemote.py, playerDataRemote.pz);
        PlayerMovementController playerCharacter = Instantiate(_characterPrefab, position, Quaternion.identity);
    }

    private void RemoveEnemy(Player player)
    {
        if (!_playersOnRoom.ContainsKey(player.__refId)) return;

        _playersOnRoom[player.__refId].Kill();
        _playersOnRoom.Remove(player.__refId);
    }   

    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnCreatePlayerLocal -= CreatePlayer;
        MultiplayerManager.Instance.OnCreateEnemy -= CreateEnemy;
        MultiplayerManager.Instance.OnRemoveEnemy -= RemoveEnemy;
    }
}
