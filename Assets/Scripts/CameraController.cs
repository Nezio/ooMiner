using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int backwardsViewDistance = 5; // how many blocks does camera see behind it's current position
    public float startingSpeed = 1f;
    public float maxSpeed = 3f;
    public int secondsToReachMaxSpeed = 20;
    public float playerFarSpeed = 10f;

    [Tooltip("!!! ONLY for debugging trough editor. Don't set value unless in play mode. Default value: 0 !!!")]
    public float speed = 0;

    public int maxPlayerDistance = 15; // how far can a player go before camera cathces up
    public GameObject player;

    private float speedupRate;
    private float previousPlayerDistanceFromCamera = 0;
    private float previousSpeed;

    private void Start()
    {
        // calculate camera speedup rate
        speedupRate = (maxSpeed - startingSpeed) / secondsToReachMaxSpeed;
    }

    void Update()
    {
        // move camera
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);


        float playerDistanceFromCamera = player.transform.position.z - transform.position.z;
        //Debug.Log(playerDistanceFromCamera);

        if (playerDistanceFromCamera >= maxPlayerDistance)
        { // player too far

            if (previousPlayerDistanceFromCamera < maxPlayerDistance)
            { // player just crossed the limit              [2 (once)]
                //Debug.Log("Player just crossed over!");

                // save previous speed and set speed to fast mode
                previousSpeed = speed;
                speed = playerFarSpeed;
            }
            else
            { // player was already over the limit          [3]
                //Debug.Log("Player across the limit!");

            }
        }
        else
        { // player not too far

            if (previousPlayerDistanceFromCamera >= maxPlayerDistance)
            { // player just crossed the limit (back)       [4 (once)]
                //Debug.Log("Player just crossed back!");

                // restore previous speed
                speed = previousSpeed;
            }
            else
            { // player was already below the limit before  [1]
                //Debug.Log("Player below the limit!");

                // increase camera speed if it started moving
                if (speed != 0 && speed < maxSpeed)
                {
                    speed += speedupRate * Time.deltaTime;
                    //Debug.Log(cameraSpeed);

                    // cap at max speed
                    if (speed > maxSpeed)
                        speed = maxSpeed;
                }
            }

        }

        // save previous distance
        previousPlayerDistanceFromCamera = playerDistanceFromCamera;

    }

    public void SetCameraSpeed(float speed)
    {
        this.speed = speed;
    }
}
