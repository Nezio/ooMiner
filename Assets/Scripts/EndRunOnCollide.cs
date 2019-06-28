using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRunOnCollide : MonoBehaviour
{
    private GameManager gm;

    private void Start()
    {
        //GameObject.FindGameObjectWithTag("GameManager")
        gm = GameObject.FindObjectOfType<GameManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Ending the run.");
            gm.EndRun();
        }

    }

}
