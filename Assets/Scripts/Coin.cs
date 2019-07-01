using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    // rotation now done trough animation
    //public float rotateSpeed = 100f;

    private void Update()
    {
        //transform.RotateAround(transform.position, transform.up, rotateSpeed * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("coin!");

            // add to player score
            other.GetComponent<Player>().AddValueToScore(coinValue);

            // set inactive
            gameObject.SetActive(false);

            // play sound
            AudioManager.instance.PlayOneShot("Coin");

        }
    }
}
