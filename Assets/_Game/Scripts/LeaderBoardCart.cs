using Colyseus.Schema;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardCart : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textScore;

    public void UpdateCart(string textName, string textScore)
    {
        _textName.text = textName;
        _textScore.text = textScore;
    }

    public void UpdateLeaderBoardScore(List<DataChange> changes)
    {
        foreach (var dataChanges in changes)
        {
            switch (dataChanges.Field)
            {
                case "score":
                    int newScore = (short)dataChanges.Value;

                    UpdateCart(_textName.text, newScore.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}
