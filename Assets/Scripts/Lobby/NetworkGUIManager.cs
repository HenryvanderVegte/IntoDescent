using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(MyNetworkLobbyManager))]
public class NetworkGUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject networkInfo;
    [SerializeField]
    private GameObject networkHUD;
    [SerializeField]
    private GameObject lobbyGUI;


    [SerializeField]
    private string placehold = "localhost";

    [SerializeField]
    private GameObject ipPlaceholder;
    [SerializeField]
    private GameObject ipTextField;
    [SerializeField]
    private Text clientInfoText;
    [SerializeField]
    private Text serverInfoText;

    private MyNetworkLobbyManager networkLobbyManager;

    private bool gameIsActive = false;



    void Awake()
    {
        networkLobbyManager = GetComponent<MyNetworkLobbyManager>();
        ipPlaceholder.GetComponent<Text>().text = placehold;
    }

    void Update()
    {
        if (gameIsActive)
        {
            if (NetworkClient.active)
            {
                clientInfoText.text = ("Client: address= " + networkLobbyManager.networkAddress + " port= " + networkLobbyManager.networkPort);
            }
            if (NetworkServer.active)
            {
                clientInfoText.text = ("Server: port= " + networkLobbyManager.networkPort);
            }
        }

    }

    public void startHostButton()
    {
        networkLobbyManager.StartHost();
   //     networkLobbyManager.showLobbyGUI = true;
        networkInfo.SetActive(true);
        networkHUD.SetActive(false);
        lobbyGUI.SetActive(true);
        gameIsActive = true;
    }

    public void startClientButton()
    {
        networkLobbyManager.StartClient();
        string ipText = ipTextField.GetComponent<Text>().text;
        if (ipText == "")
        {
            networkLobbyManager.networkAddress = placehold;
        }
        else
        {
            networkLobbyManager.networkAddress = ipText;
        }
      //  networkLobbyManager.showLobbyGUI = true;
        networkInfo.SetActive(true);
        networkHUD.SetActive(false);
        lobbyGUI.SetActive(true);
        gameIsActive = true;
    }

    public void startServerOnlyButton()
    {
        networkLobbyManager.StartServer();
   //     networkLobbyManager.showLobbyGUI = true;
        networkInfo.SetActive(true);
        networkHUD.SetActive(false);
        lobbyGUI.SetActive(true);
        gameIsActive = true;
    }

    public void DisconnectButton()
    {
        networkLobbyManager.StopHost();
        networkLobbyManager.showLobbyGUI = false;
        networkInfo.SetActive(false);
        networkHUD.SetActive(true);
        lobbyGUI.SetActive(false);
        gameIsActive = false;
    }

}
