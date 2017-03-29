using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(OverlordSetup))]
public class Overlord : NetworkBehaviour {

    public bool isActive = false;
    public GameObject GUI;

    private OverlordGUIManager overlordGUIManager;
    private OverlordGameStatusGUIManager overlordGameStatusGUIManager;


    [SyncVar]
    public int playerID = -1;

    void Start()
    {
        overlordGUIManager = GetComponent<OverlordGUIManager>();
        overlordGameStatusGUIManager = GetComponent<OverlordGameStatusGUIManager>();
    }

    public void SetActive()
    {
        CmdSetActive();
        overlordGUIManager.SetGUIActive();
    }

    [Command]
    public void CmdSetActive()
    {
        RpcSetActive();
    }

    [ClientRpc]
    public void RpcSetActive()
    {
        isActive = true;
        overlordGameStatusGUIManager.SetActiveGUI();
    }


    public void SetInactive()
    {
        CmdSetInactive();
        overlordGUIManager.SetGUIInactive();
    }

    [Command]
    public void CmdSetInactive()
    {
        RpcSetInactive();
    }

    [ClientRpc]
    public void RpcSetInactive()
    {
        isActive = false;
        overlordGameStatusGUIManager.SetInactiveGUI();
        GameManager.StartNewTurn();
    }

}
