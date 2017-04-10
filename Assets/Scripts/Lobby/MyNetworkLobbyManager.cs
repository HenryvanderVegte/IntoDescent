using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkLobbyManager : NetworkLobbyManager {

    private bool gameStarted = false;
    public int connectedLobbyPlayers = 0;

    private int playersReady = 0;

    public override void OnLobbyServerPlayersReady()
    {    
        if (LobbyManager.ContainsOverlord())
        {
            bool _allReady = true;
            foreach (LobbyPlayer _player in lobbySlots)
            {
                if (_player != null && !_player.readyToBegin)
                {
                    _allReady = false;
                }
            }
            if (_allReady)
            {
                foreach (LobbyPlayer _lobbyPlayer in lobbySlots)
                {
                    if (_lobbyPlayer != null)
                    {
                        connectedLobbyPlayers++;
                    }

                }

                Destroy(GetComponent<NetworkGUIManager>());

                gameStarted = true;
               
                base.OnLobbyServerPlayersReady();
            }
        }
        else
        {
            Debug.Log("No Overlord in game");
        }

    }


    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection _conn, short _playerControllerId)
    {
        foreach (LobbyPlayer _lobbyPlayer in lobbySlots)
        {

            if (_lobbyPlayer != null && _lobbyPlayer.connectionToClient.connectionId == _conn.connectionId)
            {
                if (_lobbyPlayer.isHero)
                {                   
                    gamePlayerPrefab = spawnPrefabs[0];
                }
                else
                {
                    gamePlayerPrefab = spawnPrefabs[1];
                }

            }
        }   
        return base.OnLobbyServerCreateGamePlayer(_conn, _playerControllerId);
    }

    public void OnPlayerReady()
    {
        playersReady++;
        if (playersReady == connectedLobbyPlayers)
        {
            //All Players are ready to play
            foreach (GameObject _player in GameObject.FindGameObjectsWithTag("Player"))
            {
                _player.GetComponent<GamePlayerSetup>().SetID();
            }
            GameObject.FindGameObjectWithTag("Overlord").GetComponent<OverlordSetup>().SetID();

        }
    }


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        foreach (GameObject _lobbyPlayer in GameObject.FindGameObjectsWithTag("LobbyPlayer"))
        {
                _lobbyPlayer.GetComponent<LobbyPlayerSetup>().CmdSyncIsHero();
                _lobbyPlayer.GetComponent<LobbyPlayerSetup>().CmdSyncIsReady();
            
        }
        base.OnServerAddPlayer(conn, playerControllerId);

    }

}
