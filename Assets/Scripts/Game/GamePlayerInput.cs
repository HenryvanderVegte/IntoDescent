using UnityEngine;
using System.Collections;

public class GamePlayerInput : MonoBehaviour {

    public Camera playerCam;

    GamePlayer gamePlayer;

    public float timer;

    private float time = 0.0f;

    public GameObject playerInfoPrefab;

    private GameObject localPlayerGUI;

    private GameObject playerInfo;

    private bool playerInfoActive = false;
    



    void Start()
    {
        if (!transform.parent.GetComponent<GamePlayer>())
        {
            Debug.LogError("Component GamePlayer not found in parent.");
        } else {
            gamePlayer = transform.parent.GetComponent<GamePlayer>();
        }
    }

    //Method called from the GameManager when all Players are registered
    public void InitiateInput()
    {
        if (GameManager.localPlayer != null)
        {
            localPlayerGUI = GameManager.localPlayer.GUI;
        }
        else
        {
            localPlayerGUI = GameManager.overlord.GUI;
        }

    }

    void OnMouseDown()
    {
        if (!gamePlayer.isLocalPlayer)
            return;
        gamePlayer.SetActive();
    }

    
    void OnMouseOver()
    {
        time += Time.deltaTime;
        Debug.Log(time);
        if (time >= timer)
        {
            if(!playerInfoActive){
                playerInfoActive = true;
                playerInfo = Instantiate(playerInfoPrefab);
                playerInfo.transform.SetParent(localPlayerGUI.transform);

            }

            PositionPlayerInfo();
        }
    }

    void OnMouseExit()
    {
          time = 0.0f;
          playerInfoActive = false;
          Destroy(playerInfo);
    }

    private void PositionPlayerInfo()
    {
        //Only works if Canvas mode is "Screen Space - Overlay"
        RectTransform playerInfoRT = (RectTransform)playerInfo.transform;
        playerInfoRT.transform.position = new Vector3(
            Input.mousePosition.x + (playerInfoRT.sizeDelta.x / 2),
            Input.mousePosition.y + (playerInfoRT.sizeDelta.y / 2),
            Input.mousePosition.z);


        
    }
    
}
