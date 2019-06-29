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
            if (!other.GetComponent<Rigidbody>().isKinematic)
                return;

            //Debug.Log("grabbing player");

            // make player safe (unable to fall off)
            other.GetComponent<Player>().MakePlayerSafe();

            // save old player parent
            oldPlayerParent = other.transform.parent;
            // set player parent to this object or platform
            other.transform.parent = platform.transform;

            // align player to the platform
            platform.GetComponent<Platform>().SnapPlayerToLocalGrid(other.gameObject);

            // enable player controlls, disable fall and reset player rotation(just in case)
            other.GetComponent<Player>().UnfreezePlayer();
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.eulerAngles = new Vector3(0, other.transform.eulerAngles.y, 0);
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
            other.transform.parent = oldPlayerParent;

            // align player to the world grid
            other.transform.position = new Vector3(Mathf.Round(other.transform.position.x), other.transform.position.y, Mathf.Round(other.transform.position.z));
        }
    }
}
