using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private string type;
    public bool isMineable = false;
    [SerializeField]
    private float health = 100f;
    public GameObject particle_block_hit = null;
    public GameObject particle_block_destroy = null;

    private float defaultHealth;

    private void Awake()
    {
        defaultHealth = health;
    }

    public string GetBlockType()
    {
        return type;
    }

    public void DamageBlock(float value, Vector3 hitPosition)
    {
        if(isMineable)
        {
            // decrease health
            health -= value;

            if (health > 0)
            { // block is still standing

                // play block hit sound
                AudioManager.instance.PlayOneShot("Block_Hit_0" + Random.Range(1, 4));

                // spawn block hit particle
                if (particle_block_hit != null)
                    Instantiate(particle_block_hit, hitPosition, Quaternion.identity);
            }
            else
            { // block is at 0 or less health

                // play destroy sound
                AudioManager.instance.PlayOneShot("Block_Break");

                // spawn destroy particles
                if (particle_block_destroy != null)
                    Instantiate(particle_block_destroy, transform.position, Quaternion.identity);

                // destroy block
                gameObject.SetActive(false);

            }
        }
    }
    
    public void ReinitializeBlock()
    {
        // set active
        gameObject.SetActive(true);

        // restore default health
        health = defaultHealth;
    }
}
