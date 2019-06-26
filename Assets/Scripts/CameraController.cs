using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int cameraBackwardsViewDistance = 5; // how many blocks does camera see behind it's current position
    public Level Level;
    
    void Update()
    {
        // move camera slowly forward

        // check if new blocks should be generated
        Level.GenerateCheck();
    }
}
