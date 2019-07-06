using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public int leftBoundary = -4;
    public int rightBoundary = 4;
    public GameManager gameManager;

    private Player player;
    private Animator playerAnimator;

    private bool allowMoveForward = true;
    private bool allowMoveBack = true;
    private bool allowMoveLeft = true;
    private bool allowMoveRight = true;

    private bool playerOnLeftBound = false;
    private bool playerOnRightBound = false;

    private bool firstPlayerMove = true;


    private void Start()
    {
        player = gameObject.GetComponent<Player>();
        playerAnimator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        if(!player.ControlsFrozen())
        { // only allow controls if player isn't frozen 

            // android controls ------------------------------------------------------

            if(SwipeInput.swipedUp || SwipeInput.tap)
            {
                MoveForward();
            }
            else if(SwipeInput.swipedDown)
            {
                MoveBack();
            }
            else if (SwipeInput.swipedLeft)
            {
                MoveLeft();
            }
            else if (SwipeInput.swipedRight)
            {
                MoveRight();
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
        if (player.transform.position.x <= leftBoundary)
            playerOnLeftBound = true;
        else
            playerOnLeftBound = false;

        if (player.transform.position.x >= rightBoundary)
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
            player.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 10));
            // move forward
            Vector3 destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
            StartCoroutine(MovePlayerToPosition(destination, player.moveSpeed));

            // increment score
            player.AddValueToScore(1);
        }
    }
    private void MoveBack()
    {
        if(allowMoveBack)
        {
            OnPlayerMoved();

            // rotate to face back
            player.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10));
            // move back
            Vector3 destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1);
            StartCoroutine(MovePlayerToPosition(destination, player.moveSpeed));

            // decrement score
            player.AddValueToScore(-1);
        }
    }
    private void MoveLeft()
    {
        if(allowMoveLeft && !playerOnLeftBound)
        {
            OnPlayerMoved();

            // rotate to face left
            player.transform.LookAt(new Vector3(player.transform.position.x - 10, player.transform.position.y, player.transform.position.z));
            // move left
            Vector3 destination = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
            StartCoroutine(MovePlayerToPosition(destination, player.moveSpeed));
            
        }
    }
    private void MoveRight()
    {
        if(allowMoveRight && !playerOnRightBound)
        {
            OnPlayerMoved();
            
            // rotate to face right
            player.transform.LookAt(new Vector3(player.transform.position.x + 10, player.transform.position.y, player.transform.position.z));
            // move right
            Vector3 destination = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
            StartCoroutine(MovePlayerToPosition(destination, player.moveSpeed));
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

        Vector3 startPosition = player.transform.position;

        player.FreezePlayerControls();
        player.SetPlayerMoving(true);

        // play walk animation; if animation transitions were present animation parametars would also have to be used to switch animations properly
        playerAnimator.Play("Walk", 0, Random.Range(0f, 1f));

        float t = 0f;
        while (t < 1f)
        {
            t = t + Time.deltaTime * speed;
            Vector3 newPosition = Vector3.Lerp(startPosition, destination, t);
            player.transform.position = newPosition;

            /* this is to prevent player scale change while lerping in local space; ideally player should not use world space to lerp 
             * while in local space as this could cause the player to move at the same speed as the platform beneath and never acctually move
             * relative to the platform. However this inmplementation gives satisfying results for now. */
            // unparent
            Transform oldParent = player.transform.parent;
            player.transform.SetParent(null);
            player.transform.localScale = player.GetScale();
            // parent back
            player.transform.SetParent(oldParent);
            //Debug.Log(player.transform.localScale);
            //Debug.Log(player.transform.lossyScale);
            
            yield return new WaitForEndOfFrame();
        }
        // MOVE ENDED


        player.SetPlayerMoving(false);

        // unfreeze if player not falling
        if(!player.IsFalling())
            player.UnfreezePlayerControls();
        
        // return to idle animation
        playerAnimator.CrossFade("Idle", 0f);

        if (player.IsOnPlatform())
        {
            // align player to the platform
            transform.parent.gameObject.GetComponent<Platform>().SnapPlayerToLocalGrid(gameObject);
            // enable player controlls, disable fall and reset player rotation(just in case)
            player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
        }

        yield return null;
    }
    
   

    private void OnPlayerFirstMove()
    { // only call this when player starts moving for the first time in a run
        //Debug.Log("I can walk!");

        // start the run
        gameManager.StartRun();
    }

}
