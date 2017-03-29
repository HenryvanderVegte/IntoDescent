using UnityEngine;
using System.Collections;

public class PlayerMoveField : MonoBehaviour {

    void OnMouseDown()
    {
        GameManager.localPlayer.GetComponent<GamePlayerMovement>().StartMovement((int)transform.position.x, (int)transform.position.z);
    }
}
