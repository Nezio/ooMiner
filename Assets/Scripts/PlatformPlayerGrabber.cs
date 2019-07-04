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

            if (!other.GetComponent<Player>().IsFalling())
            {// only if player is not falling to an endgame

                //Debug.Log("grabbing player");

                // make player safe (unable to fall off)
                other.GetComponent<Player>().MakePlayerSafe();

                // save old player parent
                oldPlayerParent = other.transform.parent;
                // set player parent to this object or platform
                other.transform.SetParent(platform.transform);
            }
            //else
            //    Debug.Log("can't grab!");
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        { // player left the platform

            // only release the player if other platform hasn't grabbed him already
            // i.e. only release the player if he is a child of this platform (check by ID)
            if (other.transform.parent != null && other.transform.parent.GetInstanceID() == transform.parent.GetInstanceID())
            {
                //Debug.Log("releasing player to: " + (oldPlayerParent == null ? "null" : oldPlayerParent.gameObject.name) );

                // make player unsafe again
                other.GetComponent<Player>().MakePlayerUnsafe();

                // return old player parent
                //other.transform.SetParent(oldPlayerParent);
                other.transform.SetParent(null);    // fixes player not beeing released when leaving platforms after jumping platforms and having release triggered after grab

                // align player to the world grid
                StartCoroutine(other.GetComponent<Player>().AlignToWorldGrid());
            }
            //else
            //    Debug.Log("not releasing!");
            
        }
    }

}
