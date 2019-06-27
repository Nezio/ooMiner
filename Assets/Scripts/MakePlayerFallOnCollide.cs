using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerFallOnCollide : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TryMakePlayerFall(other);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            TryMakePlayerFall(other);
        }
    }

    private void TryMakePlayerFall(Collider playerCollider)
    {
        if (!playerCollider.GetComponent<Player>().GetPlayerSafety())
        { // only make player fall if he is unsafe (not on a platform)
          //Debug.Log("Making player fall!");

            // make player fall down
            playerCollider.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            // freeze player controls
            playerCollider.GetComponent<Player>().FreezePlayer();
        }


        // disable player input
    }

}
