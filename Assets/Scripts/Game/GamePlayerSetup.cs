using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(GamePlayer))]
[RequireComponent(typeof(GamePlayerProperties))]
public class GamePlayerSetup : NetworkBehaviour {

    [SerializeField]
    GameObject[] componentsToDisable;
    [SerializeField]
    Behaviour[] scriptsToDisable;

    GamePlayer gamePlayer;
    GamePlayerGUIManager gamePlayerGUIManager;
    GamePlayerProperties gamePlayerProperties;
    Camera sceneCamera;

	void Start () {
        gamePlayer = GetComponent<GamePlayer>();
        gamePlayerGUIManager = GetComponent<GamePlayerGUIManager>();
        gamePlayerProperties = GetComponent<GamePlayerProperties>();
        Setup();

	}


    public void Setup()
    {
        if (isLocalPlayer)
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            GameManager.localPlayer = gamePlayer;

            CmdSetReady();
            gamePlayerGUIManager.InitiatePlayerGUI();
        }
        else
        {
            DisableComponents();
            DisableScripts();
        }

    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].SetActive(false);
        }
    }

    void DisableScripts()
    {
        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            scriptsToDisable[i].enabled = false;
        }
    }

    [Command]
    public void CmdSetReady()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkLobbyManager>().OnPlayerReady();
    }

    [Server]
    public void SetID()
    {
        GameManager.SetConnectedPlayers();
        RpcSetConnectedPlayers();
        foreach (GameObject _lobbyPlayer in GameObject.FindGameObjectsWithTag("LobbyPlayer"))
        {
            
            if (gamePlayer.connectionToClient == _lobbyPlayer.GetComponent<LobbyPlayer>().connectionToClient)
            {
                gamePlayer.playerID = _lobbyPlayer.GetComponent<LobbyPlayer>().playerID;
                GameManager.RegisterPlayer(gamePlayer);
                RpcSetID(gamePlayer.playerID);
            }
        }
    }  

    [ClientRpc]
    public void RpcSetID(int _playerID)
    {
        if (!isServer)
        {
            gamePlayer.playerID = _playerID;
            GameManager.RegisterPlayer(gamePlayer);
        }
    }

    [ClientRpc]
    public void RpcSetConnectedPlayers()
    {
        if (isLocalPlayer)
        {
            GameManager.SetConnectedPlayers();
        }
    }
}
