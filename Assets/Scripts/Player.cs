using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    
    
    private bool playerSafe = false;
    private bool frozenControls = false;
    private bool playerMoving = false;
    private Vector3 scale;  // used to reset player scale back in case it changes (happens during lerp in local space, when player moves while on a platform)

    private void Start()
    {
        scale = transform.localScale;
    }

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
    public void FreezePlayerControls()
    {
        frozenControls = true;
    }
    public void UnfreezePlayerControls()
    {
        frozenControls = false;
    }
    public bool ControlsFrozen()
    {
        return frozenControls;
    }

    // player movement
    public void SetPlayerMoving(bool value)
    {
        playerMoving = value;
    }
    public bool IsMoving()
    {
        return playerMoving;
    }

    public IEnumerator AlignToWorldGrid()
    {
        while(playerMoving)
        {
            yield return new WaitForEndOfFrame();
        }

        // if player is no longer moving: align to the grid
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));

        yield return null;
    }

    public bool IsOnPlatform()
    {
        Platform platform = null;
        try
        { // try to get platform script from parent of player (only happens if player is on a platform)
            platform = transform.parent.gameObject.GetComponent<Platform>();
        }
        catch { }

        if (platform == null)
            return false;
        else
            return true;
    }

    public Vector3 GetScale()
    {
        return scale;
    }
}
