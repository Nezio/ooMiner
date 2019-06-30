﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerFallOnCollide : MonoBehaviour
{
    [Tooltip("Fall speed fot this collider only. Leave at a negative number to use default player fallSpeed value.")]
    public float fallSpeed = -1;

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponent<Player>().GetPlayerSafety())
            { // only make player fall if he is unsafe (not on a platform)
              //Debug.Log("Fall!");

                // make player fall down using physics (DEPRICATED)
                //playerCollider.gameObject.GetComponent<Rigidbody>().isKinematic = false;

                // make player fall down by not using physics
                other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y - fallSpeed * Time.deltaTime, other.transform.position.z);

                if(!other.GetComponent<Player>().IsFalling())
                { // only do theese once when fall begins
                    // set player falling
                    other.GetComponent<Player>().SetPlayerFalling(true);

                    // freeze player controls
                    other.GetComponent<Player>().FreezePlayerControls();
                }
                
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponent<Player>().GetPlayerSafety())
            { // only make player fall if he is unsafe (not on a platform)
              //Debug.Log("Fall!");

                if (!other.GetComponent<Player>().IsFalling())
                { // only do theese once when fall begins
                    // set player falling
                    other.GetComponent<Player>().SetPlayerFalling(true);

                    // freeze player controls
                    other.GetComponent<Player>().FreezePlayerControls();
                }

                StartCoroutine(other.GetComponent<Player>().MakePlayerFall(-1));
                
            }
        }
    }



}
