using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public AudioSource music;
    public CameraController cameraController;

    public GameObject pauseButton;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    [Tooltip("Where do you want player score to be displayed")]
    public GameObject scoreText;
    public Text gamoverScoreText;
    public GameObject tutorialMenu;

    private bool tutorialWindowActive = true;
    
    private void Start()
    {
        // start the game (same as resume)
        Resume();

        // read settings and set camera perspective

        // read settings and enable tutorial window?
    }

    public void Pause()
    {
        // freeze player
        player.FreezePlayerControls();

        // set timescale
        Time.timeScale = 0f;

        // show pause menu
        pauseMenu.SetActive(true);

        // hide pause button
        pauseButton.SetActive(false);

        // pause music
        if(music != null)
        {
            music.Pause();
        }

        // hide tutorial window
        tutorialMenu.SetActive(false);

    }

    public void Resume()
    {
        StartCoroutine(ResumeCorutine());
    }
    private IEnumerator ResumeCorutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        // unfreeze player
        player.UnfreezePlayerControls();

        // set timescale
        Time.timeScale = 1f;

        // hide pause menu
        pauseMenu.SetActive(false);

        // show pause button
        pauseButton.SetActive(true);

        // resume music
        if (music != null)
        {
            music.Play();
        }

        // show tutorial window if needed
        if (tutorialWindowActive)
            tutorialMenu.SetActive(true);

        yield return null;
    }

    public void EndRun()
    {
        // stop player fall
        player.GetComponent<Rigidbody>().isKinematic = false;

        // freeze player controls
        player.FreezePlayerControls();

        // set timescale
        Time.timeScale = 0f;

        // show game over menu
        gameOverMenu.SetActive(true);

        // hide pause button
        pauseButton.SetActive(false);

        // play sound
        AudioManager.instance.PlayOneShot("EndRun");

        // stop music
        if (music != null)
        {
            music.Stop();
        }

        // hide HUD score
        scoreText.SetActive(false);

        // set score text
        gamoverScoreText.text += player.GetRunHighscore().ToString(); ;
    }

    public void StartRun()
    {
        // disable tutorial menu
        tutorialMenu.SetActive(false);
        tutorialWindowActive = false;   // used to make sure it doesn't appear in this run anymore

        // start camera movement
        cameraController.SetCameraSpeed(cameraController.defaultCameraSpeed);
    }

    public void UpdateScoreTextUI()
    {
        scoreText.GetComponent<Text>().text = player.GetRunHighscore().ToString();
    }
}
