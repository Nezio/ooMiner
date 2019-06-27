using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEventHandlerGame : MonoBehaviour
{ // responds to UI events that happen in-game and can't(or wouldn't make sense to) be handled by UIEventHandlerMenu

    public GameObject pauseMenu;
    public GameObject pauseButton;
    public Player player;

    public void Pause()
    {
        // freeze player
        player.FreezePlayer();

        // set timescale
        Time.timeScale = 0f;

        // show pause menu
        pauseMenu.SetActive(true);

        // hide pause button
        pauseButton.SetActive(false);
    }

    public void Resume()
    {
        // unfreeze player
        player.UnfreezePlayer();

        // set timescale
        Time.timeScale = 1f;

        // hide pause menu
        pauseMenu.SetActive(false);

        // show pause button
        pauseButton.SetActive(true);


    }
}
