using System.Collections.Generic;
using UnityEngine;

public class NetworkEntitySpawner : MonoBehaviour
{
    [SerializeField] private EnemyNetworkHandler _enemyNetworkHandler;
    [SerializeField] private PlayerLocal _characterPrefab;
    [SerializeField] private Enemy _enemyPrefab;

    void Start()
    {
        MultiplayerManager.Instance.OnCreatePlayerLocal += CreatePlayer;
        MultiplayerManager.Instance.OnCreateEnemy += CreateEnemy;
        MultiplayerManager.Instance.OnRemoveEnemy += RemoveEnemy;
    }

    private void CreateEnemy(Player enemyDataRemote,string sessionId)
    {
        Vector3 position = new(enemyDataRemote.movementData.px, enemyDataRemote.movementData.py, enemyDataRemote.movementData.pz);

        Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.Init(enemyDataRemote, sessionId).Forget();
        _enemyNetworkHandler.SessionIdEnemyPairsOnRoom.Add(sessionId, enemy);
    }

    private void CreatePlayer(Player playerDataRemote)
    {
        Vector3 position = new(playerDataRemote.movementData.px, playerDataRemote.movementData.py, playerDataRemote.movementData.pz);
        Vector3 euler = new(0, playerDataRemote.movementData.ry,0);

        PlayerLocal playerLocal = Instantiate(_characterPrefab, position, Quaternion.Euler(euler));
        playerLocal.Init(playerDataRemote).Forget();
    }

    private void RemoveEnemy(Player player, string sessionId)
    {
        if (!_enemyNetworkHandler.SessionIdEnemyPairsOnRoom.ContainsKey(sessionId)) return;

        _enemyNetworkHandler.SessionIdEnemyPairsOnRoom[sessionId].Kill();
        _enemyNetworkHandler.SessionIdEnemyPairsOnRoom.Remove(sessionId);
    }   

    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnCreatePlayerLocal -= CreatePlayer;
        MultiplayerManager.Instance.OnCreateEnemy -= CreateEnemy;
        MultiplayerManager.Instance.OnRemoveEnemy -= RemoveEnemy;
    }
}
