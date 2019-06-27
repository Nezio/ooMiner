using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerFallOnCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Making player fall!");
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            // disable player input
        }
        
    }

}
