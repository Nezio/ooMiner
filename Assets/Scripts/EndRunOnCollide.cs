using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRunOnCollide : MonoBehaviour
{ // attach this scritp to trigger colliders that should end run when player collides with them

    private GameManager gameManager;

    private void Start()
    {
        //GameObject.FindGameObjectWithTag("GameManager")
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        { // if player entered this collider end the run
            //Debug.Log("Ending the run.");
            gameManager.EndRun();
        }

    }

}
