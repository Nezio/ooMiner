using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool playerSafe = false;
    private bool frozen = false;
    
    // player safety
    public void MakePlayerSafe()
    {
        playerSafe = true;
    }
    public void MakePlayerUnsafe()
    {
        playerSafe = false;
    }
    public bool GetPlayerSafety()
    {
        return playerSafe;
    }

    // player freezing
    public void FreezePlayer()
    {
        frozen = true;
    }
    public void UnfreezePlayer()
    {
        frozen = false;
    }
    public bool IsFrozen()
    {
        return frozen;
    }

}
