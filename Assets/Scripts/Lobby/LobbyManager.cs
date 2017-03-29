using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour {

    public static LobbyManager instance;

    private static MyNetworkLobbyManager networkLobbyManager;

    private const string PLAYER_ID_PREFIX = "Player ";


    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
        networkLobbyManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkLobbyManager>();
    }


    public static string GetName(LobbyPlayer _lobbyPlayer)
    {
        return PLAYER_ID_PREFIX + (_lobbyPlayer.slot + 1);
    }



    public static int GetPlayerNumber(LobbyPlayer _lobbyPlayer)
    {
        int i = 1;
        foreach (LobbyPlayer _player in networkLobbyManager.lobbySlots)
        {
            if (_player != null && _player.netId == _lobbyPlayer.netId)
            {
                return i;
            }
            i++;
        }
        Debug.LogError("Player not found");
        return 0;
    }

    public static bool ContainsOverlord()
    {
        bool _containsOverlord = false;
        foreach (LobbyPlayer _lobbyPlayer in networkLobbyManager.lobbySlots)
        {
            if (_lobbyPlayer != null && !_lobbyPlayer.isHero)
            {
                _containsOverlord = true;
            }
        }
        return _containsOverlord;
    }

}
