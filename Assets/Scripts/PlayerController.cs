using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        { // move forward
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        { // move backwards
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        { // move left
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        { // move right
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
    }
}
