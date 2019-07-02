using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int cameraBackwardsViewDistance = 5; // how many blocks does camera see behind it's current position
    public float defaultCameraSpeed = 1f;
    public float maxCameraSpeed = 3f;
    public int secondsToReachMaxSpeed = 20;

    [Tooltip("!!! ONLY for debugging trough editor. Don't set value unless in play mode. Default value: 0 !!")]
    public float cameraSpeed = 0;

    private float cameraSpeedupRate;

    private void Start()
    {
        // calculate camera speedup rate
        cameraSpeedupRate = (maxCameraSpeed - defaultCameraSpeed) / secondsToReachMaxSpeed;
    }

    void Update()
    {
        // move camera slowly forward
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + cameraSpeed * Time.deltaTime);
        
        // increase camera speed if it started moving
        if(cameraSpeed != 0 && cameraSpeed < maxCameraSpeed)
        {
            cameraSpeed += cameraSpeedupRate * Time.deltaTime;
            //Debug.Log(cameraSpeed);
        }
    }

    public void SetCameraSpeed(float speed)
    {
        cameraSpeed = speed;
    }
}
