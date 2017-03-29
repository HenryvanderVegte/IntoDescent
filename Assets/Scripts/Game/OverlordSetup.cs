using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[RequireComponent(typeof(Overlord))]
public class OverlordSetup : NetworkBehaviour {

    [SerializeField]
    GameObject[] componentsToDisable;
    [SerializeField]
    Behaviour[] scriptsToDisable;
    Overlord overlord;

    Camera sceneCamera;

	void Start () {
        overlord = GetComponent<Overlord>();
        Setup();
	}


    void Setup()
    {
        if (isLocalPlayer)
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            GameManager.overlord = overlord;
            CmdSetReady();
        }
        else
        {
            DisableComponents();
            DisableScripts();
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
            if (overlord.connectionToClient == _lobbyPlayer.GetComponent<LobbyPlayer>().connectionToClient)
            {
                overlord.playerID = _lobbyPlayer.GetComponent<LobbyPlayer>().playerID;
                GameManager.RegisterOverlord(overlord);
                RpcSetID(overlord.playerID);
            }
        }
    }

    [ClientRpc]
    public void RpcSetID(int _playerID)
    {
        if (!isServer)
        {
            overlord.playerID = _playerID;
            GameManager.RegisterOverlord(overlord);
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


}
