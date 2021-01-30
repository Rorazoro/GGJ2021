using Mirror;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenuPanel;
    [SerializeField]
    private GameObject EnterIpPanel;
    [SerializeField]
    private NetworkManagerLobby networkManager = null;

    public void OnHostButtonClick()
    {
        networkManager.StartHost();
        MainMenuPanel.SetActive(false);
    }

    public void OnJoinButtonClick()
    {
        MainMenuPanel.SetActive(false);
        EnterIpPanel.SetActive(true);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}