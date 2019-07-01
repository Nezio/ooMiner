using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed = 1f;

    public GameObject debugCube; // debug

    private Animator animator;
    private List<float> snapPositions;  // positions to which player can snap to upon jumping onto the platform
    private GameObject player;
    private Player playerScript;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

        animator = gameObject.GetComponent<Animator>();

        // set animation speed
        animator.SetFloat("Speed", speed);

        // randomize platform position
        animator.Play("Platform", 0, Random.Range(0f ,1f));

        CalculateSnapPositions();

        // debug
        //Debug.Log(transform.parent.gameObject.name + ": platform length is " + snapPositions.Count);
    }

    private void Update()
    {
        // if player somehow ended up in the wrong position, snap back
        bool playerFoundOnThisPlatform = false;
        foreach(Transform child in transform)
        { // only if player is on this platform
            if(child.tag == "Player")
                playerFoundOnThisPlatform = true;
        }
        if(playerFoundOnThisPlatform && !playerScript.IsMoving() && !playerScript.IsFalling())
        {
            bool contains = snapPositions.Contains(player.transform.localPosition.x);
            if(!contains)
                SnapPlayerToLocalGrid(player);
        }
    }

    private void CalculateSnapPositions()
    { // calculate positions to which player can snap to upon jumping onto the platform
        snapPositions = new List<float>();
        int numberOfPositions = Mathf.FloorToInt(transform.lossyScale.x);

        // used to create a local grid based on 1 unit world space grid
        float localUnit = 1 / transform.lossyScale.x;

        for (int i = 0; i < Mathf.FloorToInt(numberOfPositions / 2); i++)
        { // go trough half of the number of possible positions (because positions are symetric)
            float position;
            if ((numberOfPositions % 2) == 0)
            { // even
                position = ((i + 1) * localUnit) - (localUnit * 0.5f);
            }
            else
            { // odd
                position = (i + 1) * localUnit;
            }
            snapPositions.Add(position);
            snapPositions.Add(position * -1);                
        }

        if ((numberOfPositions % 2) == 1)
        { // if odd add the one in the middle
            snapPositions.Add(0f);
        }
        
        // debug
        /*for(int i = 0; i < snapPositions.Count; i++)
        {
            GameObject cubeInstance = GameObject.Instantiate(debugCube);
            cubeInstance.transform.SetParent(transform);
            cubeInstance.transform.localPosition = new Vector3(snapPositions[i], 1, 0);
        }*/
    }

    public void SnapPlayerToLocalGrid(GameObject player)
    { // snap player to the local grid of the platform
        // find closest point
        Vector3 playerLocPos = player.transform.localPosition;
        float closestPoint = snapPositions[0];
        for(int i = 0; i < snapPositions.Count; i++)
        {
            if(Mathf.Abs((playerLocPos.x - snapPositions[i])) < Mathf.Abs((playerLocPos.x - closestPoint)))
                closestPoint = snapPositions[i];
        }

        // snap player to it
        player.transform.localPosition = new Vector3(closestPoint, playerLocPos.y, playerLocPos.z);

    }

}
