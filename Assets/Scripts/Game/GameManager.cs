using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int maxSizeX;
    public int maxSizeY;
    public Texture2D standardCursor;
    public Texture2D onEnemyCursor;

    // 0 = Free - 1 = Player - 2 = Monster - 3 = Blocked---------------
    public int[,] ground; 

    public static GameManager instance;

    private static MyNetworkLobbyManager networkLobbyManager;
    
    public static List<GamePlayer> players = new List<GamePlayer>();
    private static int registeredPlayers = 0;

    private static int connectedPlayers = 0;
    public static Overlord overlord;

    public static GamePlayer localPlayer;

    public static bool playerRound = true; //True: Player Round - False: Overlord Round

    void Awake()
    {

        networkLobbyManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkLobbyManager>();

        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        InitiateGround();
    }

    public static void RegisterOverlord(Overlord _overlord)
    {
        string _name = "";

        foreach (GameObject _lobbyPlayerObj in GameObject.FindGameObjectsWithTag("LobbyPlayer"))
        {
            if (_lobbyPlayerObj != null)
            {
                LobbyPlayer _lobbyPlayer = _lobbyPlayerObj.GetComponent<LobbyPlayer>();

                if (_lobbyPlayer.playerID == _overlord.playerID)
                {
                    _name = _lobbyPlayer.name;
                }
            }

        }

        _name += "(Overlord)";
        _overlord.SetName(_name);
        overlord = _overlord;
        registeredPlayers++;
        if (registeredPlayers == connectedPlayers)
            InitiatePlayer();
    }

    public static void RegisterPlayer(GamePlayer _player)
    {
        string _name = "";
        foreach (GameObject _lobbyPlayerObj in GameObject.FindGameObjectsWithTag("LobbyPlayer"))
        {
            if (_lobbyPlayerObj != null)
            {
                LobbyPlayer _lobbyPlayer = _lobbyPlayerObj.GetComponent<LobbyPlayer>();

                if (_lobbyPlayer.playerID == _player.playerID)
                {
                    _name = _lobbyPlayer.name;
                }
            }
            
        }
        _name += "(Player)";
        _player.SetName(_name);
        players.Add(_player);

        //Different Start Positions
        _player.transform.position = _player.transform.position + Vector3.right * registeredPlayers;
        //Add To Ground Array
        instance.ground[(int)_player.playerObject.transform.position.x, (int)_player.playerObject.transform.position.z] = 1;

        registeredPlayers++;
        if (registeredPlayers == connectedPlayers)
            InitiatePlayer();
    }

    public static bool IsPlayerActive()
    {
        foreach (GamePlayer player in players)
        {
            if (player.isActive)
                return true;
        }

        if (overlord.isActive)
            return true;

        return false;
    }

    //Called every time a player ends his turn to check if all players did their turn
    //If every Player did his turn, the Overlord can start his turn
    public static void PlayerEndTurn()
    {
        bool _playerRound = false;
        foreach (GamePlayer _gamePlayer in players)
        {
            if (_gamePlayer.hasPlayedThisRound == false)
            {
                _playerRound = true;
            }
        }
        playerRound = _playerRound;


        if (!playerRound)
        {
            if (overlord.isLocalPlayer)
            {
                overlord.SetActive();
            }
        }

    }

    public static void OverlordEndTurn()
    {
        StartNewTurn();
    }

    public static void StartNewTurn()
    {
        playerRound = true;
        foreach (GamePlayer _gamePlayer in players)
        {
            _gamePlayer.StartNewTurn();

        }
    }

    public static void InitiatePlayer()
    {
        if (overlord.isLocalPlayer)
        {
            overlord.GetComponent<OverlordGameStatusGUIManager>().InitiateGUI();
        }
        foreach (GamePlayer _gameplayer in players)
        {
            _gameplayer.playerObject.GetComponent<GamePlayerInput>().InitiateInput();
            if (_gameplayer.isLocalPlayer)
            {
                _gameplayer.GetComponent<GamePlayerGameStatusGUIManager>().InitiateGUI();
            }
        }
    }

    public static void SetConnectedPlayers()
    {
        connectedPlayers = GameObject.FindGameObjectsWithTag("Player").Length + 1;
    }

    public void InitiateGround()
    {
        ground = new int[maxSizeX, maxSizeY];
        for (int i = 0; i < maxSizeX; i++)
        {
            for (int j = 0; j < maxSizeY; j++)
            {
                ground[i, j] = 3;
            }
        }

        foreach (GameObject _ground in GameObject.FindGameObjectsWithTag("Ground"))
        {
            Vector2 currentPos = new Vector2(_ground.transform.position.x, _ground.transform.position.z);
            Vector2 _start = _ground.GetComponent<GroundProperties>().tileSize[0] + currentPos;
            Vector2 _end = _ground.GetComponent<GroundProperties>().tileSize[1] + currentPos;

            for (int x = (int)_start.x; x <= (int)_end.x; x++)
            {
                for (int z = (int)_start.y; z <= (int)_end.y; z++)
                {
                    ground[x, z] = 0;
                }

            }
        }

    }

    public static int GetPlayerNumber(GamePlayer _player)
    {
        return players.IndexOf(_player);
    }

}
