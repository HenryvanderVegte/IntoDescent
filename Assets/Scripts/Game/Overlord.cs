using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.InteropServices;

[RequireComponent(typeof(OverlordSetup))]
public class Overlord : NetworkBehaviour {

    public bool isActive = false;
    public GameObject GUI;

    private OverlordGUIManager overlordGUIManager;
    private OverlordGameStatusGUIManager overlordGameStatusGUIManager;

    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern System.IntPtr FindWindow(System.String className, System.String windowName);

    [SyncVar]
    public int playerID = -1;

    void Start()
    {
        overlordGUIManager = GetComponent<OverlordGUIManager>();
        overlordGameStatusGUIManager = GetComponent<OverlordGameStatusGUIManager>();
    }

    public void SetName(string _name)
    {
        this.name = _name;
        if (isLocalPlayer)
        {
             System.IntPtr windowPtr = FindWindow(null, "DungeonCrawler");
             SetWindowText(windowPtr, _name);
    
        }
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
        GameManager.OverlordEndTurn();
    }

}
