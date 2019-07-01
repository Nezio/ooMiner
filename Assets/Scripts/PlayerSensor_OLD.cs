using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRICATED
public class PlayerSensor_OLD : MonoBehaviour
{ // detect collisions between player and blocks that should collide with player, then block player movement in that direction

    private GameObject player;
    private PlayerController playerController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    

    private void OnTriggerExit(Collider other)
    {
        Block block = null;

        try
        { // try to get block component; this is to determine if other is a block
            block = other.GetComponent<Block>();
        }
        catch { }

        if (block != null /*&& block.collideWithPlayer*/)
        { // if other is a block and player can collide with it
            SetAllowPlayerMovement(true);

            //playerController.CheckLeftBound(player.transform.position);
            //playerController.CheckRightBound(player.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Block block = null;

        try
        { // try to get block component; this is to determine if other is a block
            block = other.GetComponent<Block>();
        }
        catch { }

        if (block != null /*&& block.collideWithPlayer*/)
        { // if other is a block and player can collide with it
            SetAllowPlayerMovement(false);
        }

    }

    private void SetAllowPlayerMovement(bool value)
    { // allow or disable player movement in appropriate directaion based on what sensor is this; value determines if movement is going to be set on or off
        // determine what sensor is this
        // find biggest and smallest Z and X coordinates
        float biggestZ = transform.position.z;
        float smallestZ = transform.position.z;
        float biggestX = transform.position.x;
        float smallestX = transform.position.x;
        foreach (Transform sensor in transform.parent)
        {
            if (sensor.position.z > biggestZ)
                biggestZ = sensor.position.z;
            if (sensor.position.z < smallestZ)
                smallestZ = sensor.position.z;
            if (sensor.position.x > biggestX)
                biggestX = sensor.position.x;
            if (sensor.position.x < smallestX)
                smallestX = sensor.position.x;
        }

        if (transform.position.z == biggestZ)
        { // forward
            Debug.Log("Forward collision - " + (value ? "exit" : "enter"));
            playerController.SetAllowMove("forward", value);
        }
        else if (transform.position.z == smallestZ)
        { // back
            Debug.Log("Back collision - " + (value ? "exit" : "enter"));
            playerController.SetAllowMove("back", value);
        }
        else if (transform.position.x == biggestX)
        { // right
            Debug.Log("Right collision - " + (value ? "exit" : "enter"));
            playerController.SetAllowMove("right", value);
        }
        else if (transform.position.x == smallestX)
        { // left
            Debug.Log("Left collision - " + (value ? "exit" : "enter"));
            playerController.SetAllowMove("left", value);
        }
    }
}
