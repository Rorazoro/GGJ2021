using Mirror;
using TMPro;
using UnityEngine;

public class NetworkGamePlayer : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";

    [SyncVar]
    private int score = 0;

    [SerializeField]
    private TMP_Text ScoreText;
    [SerializeField]
    private Canvas canvas;

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    public string GetDisplayName()
    {
        return this.displayName;
    }

    public void IncreaseScore()
    {
        this.score += 100;
        ScoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return this.score;
    }

    public override void OnStartAuthority()
    {
        canvas.gameObject.SetActive(true);
    }
}