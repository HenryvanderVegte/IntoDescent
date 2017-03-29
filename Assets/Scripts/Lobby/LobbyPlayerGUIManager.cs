using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyPlayerGUIManager : MonoBehaviour {

    private LobbyPlayerSetup lobbyPlayerSetup;


    [SerializeField]
    private Text nameInputText;
    [SerializeField]
    private Image isReadyImage;
    [SerializeField]
    private Sprite isReadySprite;
    [SerializeField]
    private Sprite isNotReadySprite;

    [SerializeField]
    private Image isHeroImage;
    [SerializeField]
    private Sprite isHeroSprite;
    [SerializeField]
    private Sprite isOverlordSprite;
    

    public void SetLobbyPlayerSetup(LobbyPlayerSetup _lobbyPlayerSetup)
    {
        lobbyPlayerSetup = _lobbyPlayerSetup;
    }

    public void ConfirmNameButton()
    {
        lobbyPlayerSetup.CmdNameChanged(nameInputText.text);
    }

    public void IsReadyButton()
    {
        lobbyPlayerSetup.SetIsReady();
    }

    public void IsHeroButton()
    {
        lobbyPlayerSetup.SetIsHero();
    }

    public void SetIsReadyGUI(bool _isReady)
    {
        if (_isReady)
        {
            isReadyImage.sprite = isReadySprite;
        }
        else
        {
            isReadyImage.sprite = isNotReadySprite;
        }
    }

    public void SetIsHeroGUI(bool _isHero)
    {
        if (_isHero)
        {
            isHeroImage.sprite = isHeroSprite;
        }
        else
        {
            isHeroImage.sprite = isOverlordSprite;
        }
    }
}
