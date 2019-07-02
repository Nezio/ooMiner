using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float fallSpeed = 10f;
    public GameManager gameManager;

    private bool playerSafe = false;
    private bool frozenControls = false;
    private bool playerMoving = false;
    private bool falling = false;
    private Vector3 scale;  // used to reset player scale back in case it changes (happens during lerp in local space, when player moves while on a platform)
    private int score = 0;          // current score (this is decremented if player moves back)
    private int runHighscore = 0;   // highscore during this run only (this is NOT decremented if player moves back)

    private void Start()
    {
        // save player scale
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
    // player falling
    public void SetPlayerFalling(bool value)
    {
        falling = value;
    }
    public bool IsFalling()
    {
        return falling;
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

    /// <param name="speed">Set speed at 0 or less to use default player fallSpeed value.</param>
    public IEnumerator MakePlayerFall(float speed)
    {
        if(speed <= 0)
        {
            speed = fallSpeed;
        }

        while(true) // currently there is no need for exit condition as player always falls to an end game
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);

            yield return new WaitForEndOfFrame();
        }
    }

    // score
    public void AddValueToScore(int value)
    {
        // set score
        score += value;

        UpdateRunHighscore();
    }
    public void UpdateRunHighscore()
    {
        // update run highscore if needed
        if (score > runHighscore)
            runHighscore = score;

        // update score UI text
        gameManager.UpdateScoreTextUI();
    }
    public int GetRunHighscore()
    {
        return runHighscore;
    }
}
