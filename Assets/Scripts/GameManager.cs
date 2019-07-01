﻿using System.Collections;
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
    public GameObject scoreText;
    public Text gamoverScoreText;
    public GameObject tutorialMenu;

    private void Awake()
    {
        // set time scale in case it is frozen
        Time.timeScale = 1f;
    }

    private void Start()
    {
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

    }

    public void Resume()
    {
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

        // start camera movement
        cameraController.SetCameraSpeed(cameraController.defaultCameraSpeed);
    }

    public void UpdateScoreTextUI()
    {
        scoreText.GetComponent<Text>().text = player.GetRunHighscore().ToString();
    }
}
