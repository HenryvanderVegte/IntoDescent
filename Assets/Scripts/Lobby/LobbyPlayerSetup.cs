using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(LobbyPlayer))]
public class LobbyPlayerSetup : NetworkBehaviour {

    [SerializeField]
    private GameObject myPlayerGUIPrefab;
    [SerializeField]
    private GameObject otherPlayerGUIPrefab;


    private LobbyPlayer lobbyPlayer;
    private GameObject playerGUI;
    private bool isReady = false;
    [SyncVar]
    public string playerName = "Player";


    void Start()
    {
        if (lobbyPlayer.isLocalPlayer)
        {
            CmdSetPlayerID();
            playerName = LobbyManager.GetName(lobbyPlayer);
            transform.name = playerName;
            CmdSetIsHero(true);
            playerGUI = Instantiate(myPlayerGUIPrefab);
            playerGUI.transform.FindChild("Player_Name").GetComponent<Text>().text = playerName;
        }
        else
        {
            playerGUI = Instantiate(otherPlayerGUIPrefab);
            transform.name = playerName;
            playerGUI.transform.FindChild("Player_Name").GetComponent<Text>().text = playerName;

              playerGUI.GetComponent<LobbyPlayerGUIManager>().SetIsHeroGUI(lobbyPlayer.isHero);
              playerGUI.GetComponent<LobbyPlayerGUIManager>().SetIsReadyGUI(isReady);
        }

        activateGUI();
    }

    void Update()
    {

        if (playerName == "Player")
        {
            CmdSetPlayerName();
        }


    }

    [Command]
    public void CmdSyncIsReady()
    {
        RpcSyncIsReady(isReady);
    }

    [ClientRpc]
    public void RpcSyncIsReady(bool _isReady)
    {
        isReady = _isReady;
    }

    [Command]
    public void CmdSyncIsHero()
    {
        RpcSyncIsHero(lobbyPlayer.isHero);
    }

    [ClientRpc]
    public void RpcSyncIsHero(bool _isHero)
    {
        lobbyPlayer.isHero = _isHero;

    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        lobbyPlayer = GetComponent<LobbyPlayer>();
    }


    void OnDisable()
    {
        //RPCCALL
        deactivateGUI();
    }
 
    private void activateGUI()
    {
        playerGUI.GetComponent<LobbyPlayerGUIManager>().SetLobbyPlayerSetup(this);
        playerGUI.transform.SetParent( GameObject.FindGameObjectWithTag("LobbyGUI").transform);
        RectTransform playerGUIRT = (RectTransform)playerGUI.transform;

        int playerNumber = LobbyManager.GetPlayerNumber(lobbyPlayer);
        playerGUIRT.anchoredPosition = new Vector2((playerNumber - 1) * 100f + 40f, 0f);
    }


    private void deactivateGUI()
    {
        Destroy(playerGUI);
    }

    public void SetIsReady()
    {
        isReady = !isReady;
        if (isReady)
        {
            lobbyPlayer.SendReadyToBeginMessage();
        }
        else
        {
            lobbyPlayer.SendNotReadyToBeginMessage();
        }
        CmdSetIsReady(isReady);
    }

    public void SetIsHero()
    {
        if (!lobbyPlayer.isHero)
        {           
            CmdSetIsHero(true);
        }
        else
        {
            if (!LobbyManager.ContainsOverlord())
            {
              //  lobbyPlayer.isHero = false;
                CmdSetIsHero(false);
            }
        }
    }

    [Command]
    public void CmdSetPlayerID()
    {
        int _playerID = lobbyPlayer.connectionToClient.connectionId;
        lobbyPlayer.playerID = _playerID;
    }

    [Command]
    public void CmdSetIsReady(bool _isReady)
    {
        RpcSetIsReady(_isReady);
    }

    [Command]
    public void CmdSetIsHero(bool _isHero)
    {
        RpcSetIsHero(_isHero);
    }

    [Command]
    public void CmdNameChanged(string _name)
    {
        RpcSyncName(_name);
    }


    [Command]
    public void CmdSetPlayerName()
    {
        RpcSetPlayerName();
    }

    [ClientRpc]
    public void RpcSetPlayerName()
    {
        if (isLocalPlayer)
        {
            CmdNameChanged(transform.name);
        }
    }

    
    [ClientRpc]
    private void RpcSyncName(string _name)
    {
        playerName = _name;
        transform.name = playerName;
        playerGUI.transform.FindChild("Player_Name").GetComponent<Text>().text = playerName;
    }


    [ClientRpc]
    private void RpcSetIsReady(bool _isReady)
    {
        playerGUI.GetComponent<LobbyPlayerGUIManager>().SetIsReadyGUI(_isReady);
    }

    [ClientRpc]
    private void RpcSetIsHero(bool _isHero)
    {
        lobbyPlayer.isHero = _isHero;
        playerGUI.GetComponent<LobbyPlayerGUIManager>().SetIsHeroGUI(_isHero);
    }


}
