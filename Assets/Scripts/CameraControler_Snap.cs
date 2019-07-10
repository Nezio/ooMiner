using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_Snap : MonoBehaviour
{
    public int cameraBackwardsViewDistance = 5; // how many blocks does camera see behind it's current position
    public float startingSpeed = 1f;
    public float maxSpeed = 3f;
    public int secondsToReachMaxSpeed = 20;

    public int maxPlayerDistance = 15; // how far can a player go before camera cathces up
    public float maxSpeed_playerFar = 10f;  // max camera speed when player is over max distance from camera
    public int secondsToReachMaxSpeed_playerFar = 2;

    [Tooltip("!!! ONLY for debugging trough editor. Don't set value unless in play mode. Default value: 0 !!")]
    public float speed = 0;

    public GameObject player;

    private float speedupRate;
    private float speedupRate_playerFar;
    private bool playerCrossedMaxDistance = false;
    private float previousSpeed;

    private void Start()
    {
        // calculate camera speedup rate
        speedupRate = (maxSpeed - startingSpeed) / secondsToReachMaxSpeed;
    }

    void Update()
    {
        // move camera slowly forward
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);

        // increase camera speed if it started moving
        if (speed != 0 && speed < maxSpeed)
        {
            speed += speedupRate * Time.deltaTime;
            //Debug.Log(cameraSpeed);
        }
        

        float playerDistanceFromCamera = player.transform.position.z - transform.position.z;
        Debug.Log(playerDistanceFromCamera);

        if (playerDistanceFromCamera > maxPlayerDistance)
        { // player too far, speed up
            if(!playerCrossedMaxDistance)
            { // player just crossed max distance
                // set bool
                playerCrossedMaxDistance = true;

                // save previous speed
                previousSpeed = speed;

                // calculate speedup rate
                speedupRate_playerFar = (maxSpeed_playerFar - speed) / secondsToReachMaxSpeed_playerFar;
            }
            else
            { // player already over max distance
                // increase speed
                speed += speedupRate_playerFar * Time.deltaTime;
            }

            Debug.Log("Camera catch up!");
        }
        else
        { // player no longer too far, slow down
            Debug.Log("Camera slow down!");
        }


    }

    public void SetCameraSpeed(float speed)
    {
        this.speed = speed;
    }
}
