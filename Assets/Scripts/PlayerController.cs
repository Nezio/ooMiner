using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            AudioManager.instance.PlayOneShot("PlayerJump");
        }
    }
    private void MoveBack()
    {
        if(allowMoveBack)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            AudioManager.instance.PlayOneShot("PlayerJump");
        }
    }
    private void MoveLeft()
    {
        if(allowMoveLeft)
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            AudioManager.instance.PlayOneShot("PlayerJump");
        }
    }
    private void MoveRight()
    {
        if(allowMoveRight)
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            AudioManager.instance.PlayOneShot("PlayerJump");
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

}
