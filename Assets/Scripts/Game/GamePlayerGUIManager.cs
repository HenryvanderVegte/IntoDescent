using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(GamePlayerProperties))]
public class GamePlayerGUIManager : MonoBehaviour {

    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text fatigueText;
    [SerializeField]
    private Text armorText;
    [SerializeField]
    private Text movementText;
    [SerializeField]
    private Text attacksText;
    [SerializeField]
    private GameObject PlayerEndTurnGUI;
    [SerializeField]
    private GameObject PlayerStartTurnGUI;
    [SerializeField]
    private GameObject PlayerAlertGUI;


    private GamePlayerProperties gamePlayerProperties;
    private GamePlayerMovement gamePlayerMovement;
    private GamePlayer gamePlayer;


    public void InitiatePlayerGUI()
    {
        gamePlayerProperties = GetComponent<GamePlayerProperties>();
        gamePlayerMovement = GetComponent<GamePlayerMovement>();
        gamePlayer = GetComponent<GamePlayer>();

        healthText.text   = gamePlayerProperties.startHealth.ToString();
        fatigueText.text  = gamePlayerProperties.startFatigue.ToString();
        armorText.text    = gamePlayerProperties.startArmor.ToString();
        movementText.text = gamePlayerProperties.startSteps.ToString();
        attacksText.text  = gamePlayerProperties.startAttacks.ToString();
    }

    public void UpdatePlayerGUI()
    {
        healthText.text = gamePlayerProperties.currentHealth.ToString();
        fatigueText.text = gamePlayerProperties.currentFatigue.ToString();
        armorText.text = gamePlayerProperties.currentArmor.ToString();
        movementText.text = gamePlayerProperties.currentSteps.ToString();
        attacksText.text = gamePlayerProperties.currentAttacks.ToString();
    }

    public void StartTurn()
    {
        PlayerStartTurnGUI.SetActive(true);
    }

    public void RunButtonClick()
    {
        PlayerStartTurnGUI.SetActive(false);
        PlayerAlertGUI.SetActive(false);
        PlayerEndTurnGUI.SetActive(true);

        gamePlayerProperties.currentSteps = gamePlayerProperties.startSteps * 2;
        gamePlayerProperties.currentAttacks = 0;
        UpdatePlayerGUI();
        gamePlayerMovement.createMoveFields();
    }

    public void FightButtonClick()
    {
        PlayerStartTurnGUI.SetActive(false);
        PlayerAlertGUI.SetActive(false);
        PlayerEndTurnGUI.SetActive(true);

        gamePlayerProperties.currentSteps = 0;
        gamePlayerProperties.currentAttacks = 2;
        UpdatePlayerGUI();
        gamePlayerMovement.createMoveFields();
    }

    public void AdvanceButtonClick()
    {
        PlayerStartTurnGUI.SetActive(false);
        PlayerAlertGUI.SetActive(false);
        PlayerEndTurnGUI.SetActive(true);

        gamePlayerProperties.currentSteps = gamePlayerProperties.startSteps;
        gamePlayerProperties.currentAttacks = gamePlayerProperties.startAttacks;
        UpdatePlayerGUI();
        gamePlayerMovement.createMoveFields();
    }

    public void AlertButtonClick()
    {
        if (PlayerAlertGUI.activeSelf)
        {
            PlayerAlertGUI.SetActive(false);
        }
        else
        {
            PlayerAlertGUI.SetActive(true);
        }
    }

    public void EndTurnButtonClick()
    {
        gamePlayerMovement.DestroyMoveFields();
        gamePlayer.SetInactive();
        PlayerEndTurnGUI.SetActive(false);
    }

    #region Alertmode

    public void AimingButtonClick()
    {

    }

    public void EvadingButtonClick()
    {

    }

    public void HedgingButtonClick()
    {

    }

    public void RestingButtonClick()
    {

    }

    #endregion





}
