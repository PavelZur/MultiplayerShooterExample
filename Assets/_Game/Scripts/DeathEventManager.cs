
using UnityEngine;

public class DeathEventManager : MonoBehaviour
{
    [SerializeField] private DeathInfoCartPrefab _cartDeathInfoPrefab;
    [SerializeField] private Transform _transformContent;

    void Start()
    {
        MultiplayerManager.Instance.OnDeathRoomEvent += CreateDeathInfoCart;
    }

    private void CreateDeathInfoCart(string jsonInfo)
    {
        DeathInfo info = JsonUtility.FromJson<DeathInfo>(jsonInfo);

        DeathInfoCartPrefab deathInfoCard = Instantiate(_cartDeathInfoPrefab, _transformContent);
        deathInfoCard.Init(info.nameKiller, info.nameDie, info.isHead);
    }

    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnDeathRoomEvent -= CreateDeathInfoCart;
    }
}

public struct DeathInfo
{
    public string nameDie;
    public string nameKiller;
    public bool isHead;
}
