using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int leftBoundary = -4;
    public int rightBoundary = 4;

    private Player player;
    private bool allowMoveForward = true;
    private bool allowMoveBack = true;
    private bool allowMoveLeft = true;
    private bool allowMoveRight = true;
    
    private void Start()
    {
        player = gameObject.GetComponent<Player>();
    }

    private void Update()
    {
        if(!player.IsFrozen())
        { // only allow controls if player isn't frozen 

            // android controls ------------------------------------------------------
            Vector2 startPos = new Vector2(0, 0);
            Vector2 direction;
            string message;

            // Track a single touch as a direction control.
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on TouchPhase
                switch (touch.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        startPos = touch.position;
                        message = "Begun ";
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        direction = touch.position - startPos;
                        message = "Moving ";
                        break;

                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        message = "Ending ";
                        break;
                }
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
    }

    private void MoveForward()
    {
        if(allowMoveForward)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1); // depricated
            AudioManager.instance.PlayOneShot("PlayerJump");

            // rotate to face forward
            player.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 10));

            // move forward
            Vector3 destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
            StartCoroutine(MovePlayerToPosition(destination, player.speed));
        }
    }
    private void MoveBack()
    {
        if(allowMoveBack)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1); // depricated
            AudioManager.instance.PlayOneShot("PlayerJump");

            // rotate to face back
            player.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10));

            // move back
            Vector3 destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1);
            StartCoroutine(MovePlayerToPosition(destination, player.speed));
        }
    }
    private void MoveLeft()
    {
        if(allowMoveLeft)
        {
            //transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z); // depricated
            AudioManager.instance.PlayOneShot("PlayerJump");

            // if player is on left boundary prevent further movement to the left
            if (player.transform.position.x <= leftBoundary)
                allowMoveLeft = false;
            // if player is not on right boundary allow movement to the right
            if (player.transform.position.x < rightBoundary)
                allowMoveRight = true;

            // rotate to face left
            player.transform.LookAt(new Vector3(player.transform.position.x - 10, player.transform.position.y, player.transform.position.z));

            // move left
            Vector3 destination = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
            StartCoroutine(MovePlayerToPosition(destination, player.speed));
        }
    }
    private void MoveRight()
    {
        if(allowMoveRight)
        {
            //transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z); // depricated
           
            AudioManager.instance.PlayOneShot("PlayerJump");

            // if player is not on left boundary allow movement to the left
            if (player.transform.position.x > leftBoundary)
                allowMoveLeft = true;
            // if player is on right boundary prevent further movement to the right
            if (player.transform.position.x >= rightBoundary)
                allowMoveRight = false;

            // rotate to face right
            player.transform.LookAt(new Vector3(player.transform.position.x + 10, player.transform.position.y, player.transform.position.z));

            // move right
            Vector3 destination = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
            StartCoroutine(MovePlayerToPosition(destination, player.speed));
        }
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
    {
        Vector3 startPosition = player.transform.position;

        player.FreezePlayer();

        float t = 0f;
        while(t < 1f)
        {
            t = t + Time.deltaTime * speed;
            Vector3 newPosition = Vector3.Lerp(startPosition, destination, t);
            player.transform.position = newPosition;
            yield return new WaitForEndOfFrame();
        }

        player.UnfreezePlayer();

        //if platform
        Platform platform = null;
        try
        { // try to get platform script from parent of player (only happens if player is on a platform)
            platform = transform.parent.gameObject.GetComponent<Platform>();
        }
        catch { }
        if(platform != null)
        { // if player is standing on a platform
          // align player to the platform
            transform.parent.gameObject.GetComponent<Platform>().SnapPlayerToLocalGrid(gameObject);
            // enable player controlls, disable fall and reset player rotation(just in case)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
        }

        
        yield return null;
    }

}
