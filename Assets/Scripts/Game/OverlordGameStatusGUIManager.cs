using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OverlordGameStatusGUIManager : MonoBehaviour {

    public GameObject gameStatusGUI;

    public GameObject playerStatusGUIPrefab;

    public GameObject[] playersStatus;

    public GameObject overlordStatusGUIPrefab;

    [HideInInspector]
    public Image[] playersActiveImage;

    [HideInInspector]
    public Image[] playersHasPlayedImage;

    [HideInInspector]
    public GameObject overlordStatus;

    private Overlord overlord;

    [HideInInspector]
    public Image overlordActiveImage;


    void Start()
    {
        overlord = GetComponent<Overlord>();
    }


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

        if (!GameManager.overlord.isLocalPlayer)
        {
            GameManager.localPlayer.GetComponent<GamePlayerGameStatusGUIManager>().overlordActiveImage.color = Color.yellow;
        }
        else
        {
            GameManager.overlord.GetComponent<OverlordGameStatusGUIManager>().overlordActiveImage.color = Color.yellow;
        }
    }

    public void SetInactiveGUI()
    {

        if (!GameManager.overlord.isLocalPlayer)
        {
            GameManager.localPlayer.GetComponent<GamePlayerGameStatusGUIManager>().overlordActiveImage.color = Color.white;
        }
        else
        {
            GameManager.overlord.GetComponent<OverlordGameStatusGUIManager>().overlordActiveImage.color = Color.white;
        }
    }
     
}

