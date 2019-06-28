using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMover : MonoBehaviour
{ // move the level back every maxLevelDistance units to prevent overflow
    public float maxLevelDistance = 1000;
    public Level level;
    public GameObject player;

    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // if camera is far from origin call level move fn
        if(mainCamera.transform.position.z >= maxLevelDistance)
        {
            MoveLevel();
        }
    }
    
    private void MoveLevel()
    {
        // move strips
        GameObject[] strips = level.GetStrips();
        for(int i = 0; i < strips.Length; i++)
        {
            Vector3 stripLocPos = strips[i].transform.localPosition;
            strips[i].transform.localPosition = new Vector3(stripLocPos.x, stripLocPos.y, stripLocPos.z - maxLevelDistance);
        }

        // player
        Vector3 playerLocPos = player.transform.localPosition;
        player.transform.position = new Vector3(playerLocPos.x, playerLocPos.y, playerLocPos.z - maxLevelDistance);

        // camera
        Vector3 cameraLocPos = mainCamera.transform.localPosition;
        mainCamera.transform.position = new Vector3(cameraLocPos.x, cameraLocPos.y, cameraLocPos.z - maxLevelDistance);
    }
}
