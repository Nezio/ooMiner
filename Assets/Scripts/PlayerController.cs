using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        string platform = null;

#if UNITY_ANDROID
        platform = "android";
#endif

        if (platform == "android")
        { // android controls

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
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
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

}
