using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(GamePlayerProperties))]
[RequireComponent(typeof(GamePlayer))]
public class GamePlayerMovement : MonoBehaviour {  

    GamePlayerProperties gamePlayerProperties;
    GamePlayer gamePlayer;
    GamePlayerGUIManager gamePlayerGUIManager;

    GameObject playerObject;

    private bool isMoving = false;
    private int positionX, positionZ;
    private int moveGoalX, moveGoalZ;
    private List<Vector2> wayPoints;

    private bool setNewGoalVector;
    private Vector3 goalVector;
    private int stepsMade;

    // On Default the walkable ground state is only 0 (Free)
    private int walkStat = 0;

    void Start()
    {
        gamePlayerProperties = GetComponent<GamePlayerProperties>();
        gamePlayer = GetComponent<GamePlayer>();
        playerObject = gamePlayer.playerObject;
        gamePlayerGUIManager = GetComponent<GamePlayerGUIManager>();

        //If the Player can move through Units, the walkable states will be 0,1,2 (every state <2)
        if (gamePlayerProperties.canMoveThroughUnits)
        {
            walkStat = 2;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }



    public void createMoveFields()
    {
        List<Vector2> canGoTo = new List<Vector2>();

        int playerPosX = (int)playerObject.transform.position.x;
        int playerPosZ = (int)playerObject.transform.position.z;

        canGoTo.Add(new Vector2(playerPosX, playerPosZ));

        for (int i = 0; i < gamePlayerProperties.currentSteps; i++)
        {
            List<Vector2> canGoToATM = new List<Vector2>();
            foreach (Vector2 pos in canGoTo)
            {
                canGoToATM.Add(pos);
            }
            foreach (Vector2 position in canGoToATM)
            {
                if (GameManager.instance.ground[(int)position.x, (int)position.y + 1] <= walkStat && (!canGoTo.Contains(new Vector2(position.x, position.y + 1))))
                {

                    canGoTo.Add(new Vector2(position.x, position.y + 1));
                }

                if (GameManager.instance.ground[(int)position.x, (int)position.y - 1] <= walkStat && (!canGoTo.Contains(new Vector2(position.x, position.y - 1))))
                {

                    canGoTo.Add(new Vector2(position.x, position.y - 1));
                }

                if (GameManager.instance.ground[(int)position.x + 1, (int)position.y] <= walkStat && (!canGoTo.Contains(new Vector2(position.x + 1, position.y))))
                {

                    canGoTo.Add(new Vector2(position.x + 1, position.y));
                }
                if (GameManager.instance.ground[(int)position.x - 1, (int)position.y] <= walkStat && (!canGoTo.Contains(new Vector2(position.x - 1, position.y))))
                {

                    canGoTo.Add(new Vector2(position.x - 1, position.y));
                }

                //------------Diagonal Movement
                if (gamePlayerProperties.canMoveDiagonal)
                {
                    if (GameManager.instance.ground[(int)position.x + 1, (int)position.y + 1] <= walkStat && (!canGoTo.Contains(new Vector2(position.x + 1, position.y + 1))))
                    {

                        canGoTo.Add(new Vector2(position.x + 1, position.y + 1));
                    }
                    if (GameManager.instance.ground[(int)position.x + 1, (int)position.y - 1] <= walkStat && (!canGoTo.Contains(new Vector2(position.x + 1, position.y - 1))))
                    {

                        canGoTo.Add(new Vector2(position.x + 1, position.y - 1));
                    }
                    if (GameManager.instance.ground[(int)position.x - 1, (int)position.y + 1] <= walkStat && (!canGoTo.Contains(new Vector2(position.x - 1, position.y + 1))))
                    {

                        canGoTo.Add(new Vector2(position.x - 1, position.y + 1));
                    }
                    if (GameManager.instance.ground[(int)position.x - 1, (int)position.y - 1] <= walkStat && (!canGoTo.Contains(new Vector2(position.x - 1, position.y - 1))))
                    {

                        canGoTo.Add(new Vector2(position.x - 1, position.y - 1));
                    }
                }
                //-------------Diagonal Movement

            }
        }

        canGoTo.Remove(new Vector2(playerPosX, playerPosZ));

        foreach (Vector2 position in canGoTo)
        {
            //Move Field should only be created, if the Field is free (0) - the player can still walk through enemies if walkStat > 0
            if (GameManager.instance.ground[(int)position.x, (int)position.y] == 0)
            {
                Instantiate(gamePlayerProperties.playerMoveField, new Vector3(position.x, 0.001f, position.y), Quaternion.identity);
            }

        }

    }

    public void StartMovement(int _moveGoalX, int _moveGoalZ)
    {
        //Prepare Movement if the character isnt moving at the moment
        if (isMoving == false)
        {
            
            positionX = (int)playerObject.transform.position.x;
            positionZ = (int)playerObject.transform.position.z;
            gamePlayer.GroundRemovePos(positionX,positionZ);
            moveGoalX = _moveGoalX;
            moveGoalZ = _moveGoalZ;
            gamePlayer.GroundSetPos(moveGoalX, moveGoalZ);

            setNewGoalVector = true;
            wayPoints = new List<Vector2>();

            SetMovementPoints();

            isMoving = true;
        }
    }


    private void Move()
    {
        if (setNewGoalVector)
        {
            Vector2 goalVector2 = wayPoints[wayPoints.Count - 1];
            goalVector = new Vector3(goalVector2.x, playerObject.transform.position.y, goalVector2.y);
            wayPoints.RemoveAt(wayPoints.Count - 1);
            setNewGoalVector = false;
        }

        playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, goalVector, 0.1f);

        if (Mathf.Abs(playerObject.transform.position.x - goalVector.x) < 0.1 && Mathf.Abs(playerObject.transform.position.z - goalVector.z) < 0.1)
        {
            if ((int)goalVector.x == moveGoalX && (int)goalVector.z == moveGoalZ) //Final Goal is reached
            {
                playerObject.transform.position = goalVector;
                EndMovement();
            }
            else
            {
                setNewGoalVector = true;
            }
        }
    }

    private void EndMovement()
    {

        DestroyMoveFields();

        gamePlayerProperties.currentSteps -= stepsMade;
        gamePlayerGUIManager.UpdatePlayerGUI();
        createMoveFields();
        isMoving = false;
    }

    public void DestroyMoveFields()
    {
        foreach (GameObject moveField in GameObject.FindGameObjectsWithTag("PlayerMoveField"))
        {
            Destroy(moveField);
        }
    }

    private void SetMovementPoints()
    {
        for (int i = 0; i < gamePlayerProperties.currentSteps; i++)
        {
            if (findWayPath(positionX, positionZ, 0, i))
            {
                break;
            }
        }
        stepsMade = wayPoints.Count;
    }

    private bool findWayPath(int posX, int posZ, int currentSteps, int maxStepsLocal)
    {

        if (GameManager.instance.ground[posX + 1, posZ] <= walkStat)
        {
            if (helpFindWayPath(posX, posZ, 1, 0, currentSteps, maxStepsLocal))
            {
                return true;
            }
        }


        if (GameManager.instance.ground[posX - 1, posZ] <= walkStat)
        {
            if (helpFindWayPath(posX, posZ, -1, 0, currentSteps, maxStepsLocal))
            {
                return true;
            }
        }


        if (GameManager.instance.ground[posX, posZ + 1] <= walkStat)
        {
            if (helpFindWayPath(posX, posZ, 0, 1, currentSteps, maxStepsLocal))
            {
                return true;
            }
        }

        if (GameManager.instance.ground[posX, posZ - 1] <= walkStat)
        {
            if (helpFindWayPath(posX, posZ, 0, -1, currentSteps, maxStepsLocal))
            {
                return true;
            }
        }

        //------------Diagonal Movement------------
        if (gamePlayerProperties.canMoveDiagonal)
        {
            if (GameManager.instance.ground[posX + 1, posZ + 1] <= walkStat)
            {
                if (helpFindWayPath(posX, posZ, 1, 1, currentSteps, maxStepsLocal))
                {
                    return true;
                }
            }

            if (GameManager.instance.ground[posX + 1, posZ - 1] <= walkStat)
            {
                if (helpFindWayPath(posX, posZ, 1, -1, currentSteps, maxStepsLocal))
                {
                    return true;
                }
            }

            if (GameManager.instance.ground[posX - 1, posZ + 1] <= walkStat)
            {
                if (helpFindWayPath(posX, posZ, -1, 1, currentSteps, maxStepsLocal))
                {
                    return true;
                }
            }

            if (GameManager.instance.ground[posX - 1, posZ - 1] <= walkStat)
            {
                if (helpFindWayPath(posX, posZ, -1, -1, currentSteps, maxStepsLocal))
                {
                    return true;
                }
            }
        }
        // ---------Diagonal Movement-----------
        return false;
    }

    private bool helpFindWayPath(int posX, int posZ, int plusX, int plusZ, int currentSteps, int maxStepsLocal)
    {
        if (GameManager.instance.ground[posX + plusX, posZ + plusZ] <= walkStat)
        {
            if (moveGoalX == posX + plusX && moveGoalZ == posZ + plusZ)
            {
                wayPoints.Add(new Vector2(posX + plusX, posZ + plusZ));
                return true;
            }
            else if (currentSteps < maxStepsLocal)
            {

                if (findWayPath(posX + plusX, posZ + plusZ, currentSteps + 1, maxStepsLocal))
                {
                    wayPoints.Add(new Vector2(posX + plusX, posZ + plusZ));
                    return true;
                }
            }
        }
        return false;
    }

}
