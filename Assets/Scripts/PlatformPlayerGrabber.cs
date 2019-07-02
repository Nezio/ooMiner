using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayerGrabber : MonoBehaviour
{ // grabs player when he jumps on the platform (parents player to the platform and some other stuff)
    private GameObject platform;    // platform should be parent of this object
    private Transform oldPlayerParent = null;

    private void Start()
    {
        platform = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        { // player entered the platform
            // don't do anything if player is falling to an end game
            if (other.GetComponent<Player>().IsFalling())
                return;

            //Debug.Log("grabbing player");

            // make player safe (unable to fall off)
            other.GetComponent<Player>().MakePlayerSafe();

            // save old player parent
            oldPlayerParent = other.transform.parent;
            // set player parent to this object or platform
            other.transform.SetParent(platform.transform);

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        { // player left the platform
            //Debug.Log("releasing player");

            // make player unsafe again
            other.GetComponent<Player>().MakePlayerUnsafe();

            // return old player parent
            other.transform.SetParent(oldPlayerParent);

            // align player to the world grid
            StartCoroutine(other.GetComponent<Player>().AlignToWorldGrid());
        }
    }
}
