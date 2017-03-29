using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePlayerGameStatusGUIManager : MonoBehaviour {

    public GameObject gameStatusGUI;

    public GameObject playerStatusGUIPrefab;

    public GameObject overlordStatusGUIPrefab;

    public Sprite hasPlayed_True;

    public Sprite hasPlayed_False;

    private GameObject[] playersStatus;

    private Image[] playersActiveImage;

    private Image[] playersHasPlayedImage;

    private GameObject overlordStatus;

    private GamePlayer gamePlayer;

    private Image isActiveImage;

    private int playerNumber;

    [HideInInspector]
    public Image overlordActiveImage;

    void Start()
    {
        gamePlayer = GetComponent<GamePlayer>();
        

    }

    //Only exectued on local Player
    public void InitiateGUI()
    {
        overlordStatus = Instantiate(overlordStatusGUIPrefab);
        overlordStatus.transform.SetParent(gameStatusGUI.transform);
        Text overlordName = overlordStatus.GetComponentInChildren<Text>();
        overlordName.text = GameManager.overlord.name;
        RectTransform overlordGUIRT = (RectTransform)overlordStatus.transform;
        overlordGUIRT.anchoredPosition = new Vector2(0f, 0f);

        overlordActiveImage = overlordStatus.transform.FindChild("IsActiveImage").GetComponent<Image>();


        playersStatus = new GameObject[GameManager.players.Count];
        playersActiveImage = new Image[GameManager.players.Count];
        playersHasPlayedImage = new Image[GameManager.players.Count];
        for (int i = 0; i < playersStatus.Length; i++)
        {
            playersStatus[i] = Instantiate(playerStatusGUIPrefab);
            playersStatus[i].transform.SetParent(gameStatusGUI.transform);
            Text playerName = playersStatus[i].GetComponentInChildren<Text>();
            playerName.text = GameManager.players[i].name;
            RectTransform playerGUIRT = (RectTransform)playersStatus[i].transform;
            playerGUIRT.anchoredPosition = new Vector2(0f, -20f * i - 30f);

            playersActiveImage[i] = playersStatus[i].transform.FindChild("IsActiveImage").GetComponent<Image>();
            playersHasPlayedImage[i] = playersStatus[i].transform.FindChild("HasPlayedImage").GetComponent<Image>();
        }
    }

    public void SetActiveGUI()
    {
        playerNumber = GameManager.GetPlayerNumber(gamePlayer);

        if (!GameManager.overlord.isLocalPlayer)
        {
            GameManager.localPlayer.GetComponent<GamePlayerGameStatusGUIManager>().playersActiveImage[playerNumber].color = Color.yellow;      
        }
        else
        {
            GameManager.overlord.GetComponent<OverlordGameStatusGUIManager>().playersActiveImage[playerNumber].color = Color.yellow;         
        }
    }

    public void SetInactiveGUI()
    {
        playerNumber = GameManager.GetPlayerNumber(gamePlayer);

        if (!GameManager.overlord.isLocalPlayer)
        {
            GameManager.localPlayer.GetComponent<GamePlayerGameStatusGUIManager>().playersActiveImage[playerNumber].color = Color.white;
            GameManager.localPlayer.GetComponent<GamePlayerGameStatusGUIManager>().playersHasPlayedImage[playerNumber].sprite = hasPlayed_True;
        }
        else
        {
            GameManager.overlord.GetComponent<OverlordGameStatusGUIManager>().playersActiveImage[playerNumber].color = Color.white;
            GameManager.overlord.GetComponent<OverlordGameStatusGUIManager>().playersHasPlayedImage[playerNumber].sprite = hasPlayed_True;
        }
    }

    //Executed when a new round starts, so that the GUI shows that no one has played this round yet
    public void ResetHasPlayedImage()
    {
        playerNumber = GameManager.GetPlayerNumber(gamePlayer);
        if (!GameManager.overlord.isLocalPlayer)
        {
            GameManager.localPlayer.GetComponent<GamePlayerGameStatusGUIManager>().playersHasPlayedImage[playerNumber].sprite = hasPlayed_False;
        }
        else
        {
            GameManager.overlord.GetComponent<OverlordGameStatusGUIManager>().playersHasPlayedImage[playerNumber].sprite = hasPlayed_False;
        }
    }

}
