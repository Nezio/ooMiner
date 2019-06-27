using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRunOnCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Ending the run.");
        }

    }

}
