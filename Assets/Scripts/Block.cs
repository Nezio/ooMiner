using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isMineable = false;

    [SerializeField]
    private string type;

    // spawn chance here

    // public bool collideWithPlayer = true; // DEPRICATED; if you want a block to block the player attach CollideWithPlayer script instead

    public string GetBlockType()
    {
        return type;
    }

}
