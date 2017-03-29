using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(GamePlayerSetup))]
public class GamePlayer : NetworkBehaviour {

    public GameObject playerObject;
    public GameObject GUI;

    public bool isActive;
    public bool hasPlayedThisRound;

    [SyncVar] 
    public int playerID = -1;

    GamePlayerSetup gamePlayerSetup;
    GamePlayerGUIManager gamePlayerGUIManager;
    GamePlayerGameStatusGUIManager gamePlayerGameStatusGUIManager;
    GamePlayerMovement gamePlayerMovement;

    void Start()
    {
        isActive = false;
        hasPlayedThisRound = false;
        gamePlayerSetup = GetComponent<GamePlayerSetup>();
        gamePlayerGameStatusGUIManager = GetComponent<GamePlayerGameStatusGUIManager>();

        if (isLocalPlayer)
        {
            gamePlayerGUIManager = GetComponent<GamePlayerGUIManager>();
            gamePlayerMovement = GetComponent<GamePlayerMovement>();
           
        }
    }


    public void StartNewTurn()
    {
        hasPlayedThisRound = false;
        gamePlayerGameStatusGUIManager.ResetHasPlayedImage();
    }

    //Can only be called from local player--
    public void SetActive()
    {
        if (!hasPlayedThisRound && !GameManager.IsPlayerActive() && GameManager.playerRound)
        {
            CmdSetActive();
            StartTurn();
        }
    }

    [Command]
    public void CmdSetActive()
    {
        RpcSetActive();
        RpcUpdateGUI();
    }

    [ClientRpc]
    public void RpcSetActive()
    {
        isActive = true;
        hasPlayedThisRound = true;
        gamePlayerGameStatusGUIManager.SetActiveGUI();
    }

    //Can only be called from local player--
    public void SetInactive()
    {
        CmdSetInactive();
    }

    [Command]
    public void CmdSetInactive()
    {
        RpcSetInactive();
        RpcUpdateGUI();
    }

    [ClientRpc]
    public void RpcSetInactive()
    {
        isActive = false;
        
        gamePlayerGameStatusGUIManager.SetInactiveGUI();
        GameManager.PlayerEndTurn();
    }


    private void StartTurn()
    {
        gamePlayerGUIManager.StartTurn();
    }



    [Command]
    public void CmdUpdateGUI()
    {
        RpcUpdateGUI();
    }


    [ClientRpc]
    public void RpcUpdateGUI()
    {
            //UPDATE GUI
    }

    #region groundUpdate
    // Removes the Player state of a ground position and changes it to 0 (Free)
    public void GroundRemovePos(int posX, int posZ)
    {
        CmdGroundRemovePos(posX, posZ);
    }

    [Command]
    public void CmdGroundRemovePos(int posX, int posZ)
    {
        RpcGroundRemovePos(posX, posZ);
    }


    [ClientRpc]
    public void RpcGroundRemovePos(int posX, int posZ)
    {
        GameManager.instance.ground[posX, posZ] = 0;
       // GameManager.instance.ground[(int)playerObject.transform.position.x, (int)playerObject.transform.position.z] = 0;
    }


    // Sets the Player state of a ground position to 1 (Player)
    public void GroundSetPos(int posX, int posZ)
    {
        CmdGroundSetPos(posX, posZ);
    }

    [Command]
    public void CmdGroundSetPos(int posX, int posZ)
    {
        RpcGroundSetPos(posX,posZ);
    }


    [ClientRpc]
    public void RpcGroundSetPos(int posX, int posZ)
    {
        GameManager.instance.ground[posX, posZ] = 1;
    }
    #endregion
}
