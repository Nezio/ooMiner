using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isMineable = false;

    [SerializeField]
    private string type;

    [SerializeField]
    private float health = 100f;

    // spawn chance here

    // public bool collideWithPlayer = true; // DEPRICATED; if you want a block to block the player attach CollideWithPlayer script instead

    public string GetBlockType()
    {
        return type;
    }

    public void DamageBlock(float value)
    {
        if(health > 0)
        {
            health -= value;

            TryDestroyBlock();
        }
    }

    private void TryDestroyBlock()
    {
        Debug.Log("Block health: " + health);

        if(health <= 0)
        { // destroy block
            Debug.Log("Disable block!");

            // spawn destroy particles


            // play destroy sound


            // disable block
            gameObject.SetActive(false);

        }
    }

}
