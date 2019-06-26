using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 10;
    public int cameraBackwardsViewDistance = 5; // how many blocks does camera see behind it's current position
    
    void Update()
    {
        // move camera slowly forward
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + cameraSpeed * Time.deltaTime);
        
    }
}
