
using UnityEngine;

public class DeathEventManager : MonoBehaviour
{
    [SerializeField] private DeathInfoCartPrefab _cartDeathInfoPrefab;
    [SerializeField] private Transform _transformContent;
    private AlphaController _alphaController;
    void Start()
    {
        _alphaController = _transformContent.GetComponent<AlphaController>();
        MultiplayerManager.Instance.OnDeathRoomEvent += CreateDeathInfoCart;
    }

    private void CreateDeathInfoCart(string jsonInfo)
    {
        DeathInfo info = JsonUtility.FromJson<DeathInfo>(jsonInfo);

        DeathInfoCartPrefab deathInfoCard = Instantiate(_cartDeathInfoPrefab, _transformContent);
        deathInfoCard.Init(info.nameKiller, info.nameDie, info.isHead);
        _alphaController.AddElement(deathInfoCard.GetComponent<CanvasGroup>());
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
