using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public int leftBoundary = -4;
    public int rightBoundary = 4;
    public GameManager gameManager;

    private Player playerScript;
    private Animator playerAnimator;

    private bool allowMoveForward = true;
    private bool allowMoveBack = true;
    private bool allowMoveLeft = true;
    private bool allowMoveRight = true;

    private bool playerOnLeftBound = false;
    private bool playerOnRightBound = false;

    private bool firstPlayerMove = true;


    // debug
    public Text debugText;


    private void Start()
    {
        playerScript = gameObject.GetComponent<Player>();
        playerAnimator = playerScript.GetComponent<Animator>();
    }

    private void Update()
    {
        if(!playerScript.ControlsFrozen())
        { // only allow controls if player isn't frozen 

            // android controls ------------------------------------------------------
            if(AndroidInput.swipedUp || AndroidInput.tap)
            {
                MoveForward();
            }
            else if(AndroidInput.swipedDown)
            {
                MoveBack();
            }
            else if (AndroidInput.swipedLeft)
            {
                MoveLeft();
            }
            else if (AndroidInput.swipedRight)
            {
                MoveRight();
            }
            else if(AndroidInput.hold)
            {
                OnHold();
            }

            // windwos/editor controls ------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.W))
            { // move forward
                MoveForward();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            { // move backwards
                MoveBack();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            { // move left
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            { // move right
                MoveRight();
            }
        }
        
        // still in update; check if player reached level bounds
        if (playerScript.transform.position.x <= leftBoundary)
            playerOnLeftBound = true;
        else
            playerOnLeftBound = false;

        if (playerScript.transform.position.x >= rightBoundary)
            playerOnRightBound = true;
        else
            playerOnRightBound = false;
        
    }

    public void MoveForward()
    {
        if(allowMoveForward)
        {
            OnPlayerMoved();
    
            // rotate to face forward
            playerScript.transform.LookAt(new Vector3(playerScript.transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z + 10));
            // move forward
            Vector3 destination = new Vector3(playerScript.transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z + 1);
            StartCoroutine(MovePlayerToPosition(destination, playerScript.moveSpeed));

            // increment score
            playerScript.AddValueToScore(1);
        }
    }
    private void MoveBack()
    {
        if(allowMoveBack)
        {
            OnPlayerMoved();

            // rotate to face back
            playerScript.transform.LookAt(new Vector3(playerScript.transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z - 10));
            // move back
            Vector3 destination = new Vector3(playerScript.transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z - 1);
            StartCoroutine(MovePlayerToPosition(destination, playerScript.moveSpeed));

            // decrement score
            playerScript.AddValueToScore(-1);
        }
    }
    private void MoveLeft()
    {
        if(allowMoveLeft && !playerOnLeftBound)
        {
            OnPlayerMoved();

            // rotate to face left
            playerScript.transform.LookAt(new Vector3(playerScript.transform.position.x - 10, playerScript.transform.position.y, playerScript.transform.position.z));
            // move left
            Vector3 destination = new Vector3(playerScript.transform.position.x - 1, playerScript.transform.position.y, playerScript.transform.position.z);
            StartCoroutine(MovePlayerToPosition(destination, playerScript.moveSpeed));
            
        }
    }
    private void MoveRight()
    {
        if(allowMoveRight && !playerOnRightBound)
        {
            OnPlayerMoved();
            
            // rotate to face right
            playerScript.transform.LookAt(new Vector3(playerScript.transform.position.x + 10, playerScript.transform.position.y, playerScript.transform.position.z));
            // move right
            Vector3 destination = new Vector3(playerScript.transform.position.x + 1, playerScript.transform.position.y, playerScript.transform.position.z);
            StartCoroutine(MovePlayerToPosition(destination, playerScript.moveSpeed));
        }
    }
    private void OnPlayerMoved()
    { // player has moved in some direction; use this function for direction independent code
      // play sound
        AudioManager.instance.PlayOneShot("PlayerJump");

        // on first player move
        if (firstPlayerMove)
        {
            firstPlayerMove = false;

            OnPlayerFirstMove();
        }

        //Debug.Log("moved!");
    }
    
    public void CheckLeftBound(Vector3 playerPosition)
    {
        // if player is on left boundary prevent further movement to the left
        if (playerPosition.x <= leftBoundary)
            allowMoveLeft = false;
        // if player is not on right boundary anymore allow movement to the right
        if (playerPosition.x < rightBoundary)
            allowMoveRight = true;
    }
    public void CheckRightBound(Vector3 playerPosition)
    {
        // if player is on right boundary prevent further movement to the right
        if (playerPosition.x >= rightBoundary)
            allowMoveRight = false;
        // if player is not on left boundary anymore allow movement to the left
        if (playerPosition.x > leftBoundary)
            allowMoveLeft = true;
    }
    
    public void SetAllowMove(string direction, bool value)
    {
        if(direction.ToLower() == "forward")
        {
            allowMoveForward = value;
        }
        else if (direction.ToLower() == "back")
        {
            allowMoveBack = value;
        }
        else if (direction.ToLower() == "left")
        {
            allowMoveLeft = value;
        }
        else if (direction.ToLower() == "right")
        {
            allowMoveRight = value;
        }
    }

    private IEnumerator MovePlayerToPosition(Vector3 destination, float speed)
    { // move player smoothly to destination

        Vector3 startPosition = playerScript.transform.position;

        playerScript.FreezePlayerControls();
        playerScript.SetPlayerMoving(true);

        // play walk animation; if animation transitions were present animation parametars would also have to be used to switch animations properly
        playerAnimator.Play("Walk", 0, Random.Range(0f, 1f));

        float t = 0f;
        while (t < 1f)
        {
            t = t + Time.deltaTime * speed;
            Vector3 newPosition = Vector3.Lerp(startPosition, destination, t);
            playerScript.transform.position = newPosition;

            /* this is to prevent player scale change while lerping in local space; ideally player should not use world space to lerp 
             * while in local space as this could cause the player to move at the same speed as the platform beneath and never acctually move
             * relative to the platform. However this inmplementation gives satisfying results for now. */
            // unparent
            Transform oldParent = playerScript.transform.parent;
            playerScript.transform.SetParent(null);
            playerScript.transform.localScale = playerScript.GetScale();
            // parent back
            playerScript.transform.SetParent(oldParent);
            //Debug.Log(player.transform.localScale);
            //Debug.Log(player.transform.lossyScale);
            
            yield return new WaitForEndOfFrame();
        }
        // MOVE ENDED


        playerScript.SetPlayerMoving(false);

        // unfreeze if player not falling
        if(!playerScript.IsFalling())
            playerScript.UnfreezePlayerControls();
        
        // return to idle animation
        playerAnimator.CrossFade("Idle", 0f);

        if (playerScript.IsOnPlatform())
        {
            // align player to the platform
            transform.parent.gameObject.GetComponent<Platform>().SnapPlayerToLocalGrid(gameObject);
            // enable player controlls, disable fall and reset player rotation(just in case)
            playerScript.transform.eulerAngles = new Vector3(0, playerScript.transform.eulerAngles.y, 0);
        }

        yield return null;
    }
    
    private void OnPlayerFirstMove()
    { // only call this when player starts moving for the first time in a run
        //Debug.Log("I can walk!");

        // start the run
        gameManager.StartRun();
    }

    private void OnHold()
    {
        if(!playerScript.IsDigging())
        {
            Ray raycast;

            // if there is a touch (mobile) use touch for a raycast, else use a mouse position
            if (Input.touches.Length > 0)
                raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            else
                raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                //debugText.text = raycastHit.collider.gameObject + "\n" + debugText.text;
                //Debug.Log(raycastHit.collider.gameObject);

                playerScript.TryDigBlock(raycastHit.collider.gameObject);
            }
        }
        
    }

}
