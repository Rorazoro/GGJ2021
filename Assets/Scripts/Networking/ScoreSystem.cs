using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerScoreTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerWinTexts = new TMP_Text[4];
    [SerializeField] private Button returnButton = null;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
        }
    }

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    private void Awake()
    {
        List<int> scores = Room.GamePlayers.Select(x => x.GetScore()).ToList();

        for (int i = 0; i < Room.GamePlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.GamePlayers[i].GetDisplayName();
            playerScoreTexts[i].text = scores[i].ToString();
        }

        int winnerIndex = scores.IndexOf(scores.Max());
        playerWinTexts[winnerIndex].text = "WINNER!!!";
        playerWinTexts[winnerIndex].gameObject.SetActive(true);
    }
}