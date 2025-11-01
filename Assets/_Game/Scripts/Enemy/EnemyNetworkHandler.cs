using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyNetworkHandler : MonoBehaviour
{
    public ReactiveDictionary<string, Enemy> SessionIdEnemyPairsOnRoom = new();

    private void OnBroadcastMessageHeander(string keyMessage , object data)
    {

    }
}
