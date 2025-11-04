using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderBoardCart _cartPrefab;
    [SerializeField] private Transform _transformContent;
    [SerializeField] private GameObject _panelLeaderBoard;

    private readonly Dictionary<string, LeaderBoardCart> _cardsScore = new();
    void Start()
    {
        MultiplayerManager.Instance.OnCreatePlayerLocal += AddPlayerLocalCardInLeaderBoard;
        MultiplayerManager.Instance.OnCreateEnemy += AddCardInLeaderBoard;
        MultiplayerManager.Instance.OnRemoveEnemy += RemoveCardOutLeaderBoard;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            _panelLeaderBoard.SetActive(true);
        }
        else
        {
            _panelLeaderBoard.SetActive(false);
        }
    }

    private void AddPlayerLocalCardInLeaderBoard(Player player)
    {
        LeaderBoardCart newScoreCard = Instantiate(_cartPrefab,_transformContent);
        string name = player.scoreData.name;

        _cardsScore.Add(name, newScoreCard);
        newScoreCard.UpdateCart(name, player.scoreData.score.ToString());
        player.scoreData.OnChange += (changes) => newScoreCard.UpdateLeaderBoardScore(changes);
    }

    private void AddCardInLeaderBoard(Player player, string sessionId)
    {
        LeaderBoardCart newScoreCard = Instantiate(_cartPrefab, _transformContent);
        string name = player.scoreData.name;

        _cardsScore.Add(name, newScoreCard);
        newScoreCard.UpdateCart(name, player.scoreData.score.ToString());

        player.scoreData.OnChange += (changes) => newScoreCard.UpdateLeaderBoardScore(changes);
    }

    private void RemoveCardOutLeaderBoard(Player player, string sessionId)
    {
        if (_cardsScore.ContainsKey(player.scoreData.name))
        {
            Destroy(_cardsScore[player.scoreData.name].gameObject);
            _cardsScore.Remove(player.scoreData.name);
        }
    }

    private void OnDestroy()
    {
        _cardsScore.Clear();
        MultiplayerManager.Instance.OnCreatePlayerLocal -= AddPlayerLocalCardInLeaderBoard;
        MultiplayerManager.Instance.OnCreateEnemy -= AddCardInLeaderBoard;
        MultiplayerManager.Instance.OnRemoveEnemy -= RemoveCardOutLeaderBoard;
    }
}
