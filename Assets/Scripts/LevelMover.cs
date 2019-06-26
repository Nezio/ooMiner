using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMover : MonoBehaviour
{
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
        // move stripes
        GameObject[] stripes = level.GetStripes();
        for(int i = 0; i < stripes.Length; i++)
        {
            Vector3 stripeLocPos = stripes[i].transform.localPosition;
            stripes[i].transform.localPosition = new Vector3(stripeLocPos.x, stripeLocPos.y, stripeLocPos.z - maxLevelDistance);
        }

        // player
        Vector3 playerLocPos = player.transform.localPosition;
        player.transform.position = new Vector3(playerLocPos.x, playerLocPos.y, playerLocPos.z - maxLevelDistance);

        // camera
        Vector3 cameraLocPos = mainCamera.transform.localPosition;
        mainCamera.transform.position = new Vector3(cameraLocPos.x, cameraLocPos.y, cameraLocPos.z - maxLevelDistance);
    }
}
