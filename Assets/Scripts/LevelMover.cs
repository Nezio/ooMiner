using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMover : MonoBehaviour
{ // move the level back every maxLevelDistance units to prevent overflow

    public float maxLevelDistance = 1000;
    public Level level;
    public GameObject player;

    private Camera mainCamera;
    private Player playerScript;

    private bool moving = false;

    private void Start()
    {
        mainCamera = Camera.main;
        playerScript = player.GetComponent<Player>();
    }

    private void Update()
    {
        // if not already in the moving process and camera is far from origin call level move fn
        if(!moving && mainCamera.transform.position.z >= maxLevelDistance)
        {
            StartCoroutine(MoveLevel());
        }
    }
    
    private IEnumerator MoveLevel()
    {
        moving = true;

        // if player is moving wait for frame where he's not
        while (playerScript.IsMoving())
        {
            yield return new WaitForEndOfFrame();
        } // MOVE THE LEVEL


        // move distance is now used instead of maxLevelDistance because camera may move a bit while player stops moving
        int moveDistance = Mathf.FloorToInt(mainCamera.transform.position.z);

        // move player only if player is not child of some platform of a strip 
        if (player.transform.parent == null)
        {
            Vector3 playerLocPos = player.transform.localPosition;
            player.transform.position = new Vector3(playerLocPos.x, playerLocPos.y, playerLocPos.z - moveDistance);
        }

        // move strips
        GameObject[] strips = level.GetStrips();
        for(int i = 0; i < strips.Length; i++)
        {
            Vector3 stripLocPos = strips[i].transform.localPosition;
            strips[i].transform.localPosition = new Vector3(stripLocPos.x, stripLocPos.y, stripLocPos.z - moveDistance);
        }
        
        // move camera
        Vector3 cameraLocPos = mainCamera.transform.localPosition;
        mainCamera.transform.position = new Vector3(cameraLocPos.x, cameraLocPos.y, cameraLocPos.z - moveDistance);


        moving = false;

        yield return null;
    }
}
