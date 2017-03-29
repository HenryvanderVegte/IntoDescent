using UnityEngine;
using System.Collections;

public class GamePlayerProperties : MonoBehaviour {

    private bool isOverlord;

    [Header("Start Properties")]
    public int startHealth;
    public int startFatigue;
    public int startArmor;
    public int startSteps;
    public int startAttacks;
    public bool canMoveDiagonal;
    public bool canMoveThroughUnits;

    public GameObject playerMoveField;

    [HideInInspector]
    public int currentHealth, currentFatigue, currentArmor, currentSteps, currentAttacks;

	void Start () {
        currentHealth = startHealth;
        currentFatigue = startFatigue;
        currentArmor = startArmor;
        currentSteps = startSteps;
        currentAttacks = startAttacks;

	}

}
