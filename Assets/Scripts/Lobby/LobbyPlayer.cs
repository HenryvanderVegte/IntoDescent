using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(LobbyPlayerSetup))]
public class LobbyPlayer : NetworkLobbyPlayer {

    
//    [HideInInspector]
    public bool isHero = true;
    [SyncVar]
    public int playerID = -1;
    [SyncVar]
    public Color color;
}
