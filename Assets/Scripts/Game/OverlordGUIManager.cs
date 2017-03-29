using UnityEngine;
using System.Collections;

public class OverlordGUIManager : MonoBehaviour {
    [SerializeField]
    private GameObject overlordEndTurnGUI;

    private Overlord overlord;

    void Start()
    {
        overlord = GetComponent<Overlord>();
        overlordEndTurnGUI.SetActive(false);
    }

    public void EndTurnButtonClick()
    {
        overlord.SetInactive();
    }

    public void SetGUIActive()
    {
        overlordEndTurnGUI.SetActive(true);
    }

    public void SetGUIInactive()
    {
        overlordEndTurnGUI.SetActive(false);
    }
}
