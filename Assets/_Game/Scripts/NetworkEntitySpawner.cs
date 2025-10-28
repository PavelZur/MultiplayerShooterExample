using System.Collections.Generic;
using UnityEngine;

public class NetworkEntitySpawner : MonoBehaviour
{
    [SerializeField] private PlayerLocal _characterPrefab;
    [SerializeField] private Enemy _enemyPrefab;

    private Dictionary<string,Enemy> _playersOnRoom = new();

    void Start()
    {
        MultiplayerManager.Instance.OnCreatePlayerLocal += CreatePlayer;
        MultiplayerManager.Instance.OnCreateEnemy += CreateEnemy;
        MultiplayerManager.Instance.OnRemoveEnemy += RemoveEnemy;
    }

    private void CreateEnemy(Player enemyDataRemote,string sessionId)
    {
        Vector3 position = new(enemyDataRemote.px, enemyDataRemote.py, enemyDataRemote.pz);
        Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.Init(enemyDataRemote, sessionId);
        _playersOnRoom.Add(sessionId, enemy);
    }

    private void CreatePlayer(Player playerDataRemote)
    {
        Vector3 position = new(playerDataRemote.px, playerDataRemote.py, playerDataRemote.pz);
        PlayerLocal playerLocal = Instantiate(_characterPrefab, position, Quaternion.identity);
        playerLocal.Init(playerDataRemote);
    }

    private void RemoveEnemy(Player player, string sessionId)
    {
        if (!_playersOnRoom.ContainsKey(sessionId)) return;

        _playersOnRoom[sessionId].Kill();
        _playersOnRoom.Remove(sessionId);
    }   

    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnCreatePlayerLocal -= CreatePlayer;
        MultiplayerManager.Instance.OnCreateEnemy -= CreateEnemy;
        MultiplayerManager.Instance.OnRemoveEnemy -= RemoveEnemy;
    }
}
