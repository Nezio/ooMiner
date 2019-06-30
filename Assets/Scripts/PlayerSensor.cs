using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("On what side of the player is this sensor. (front, back, left, right)")]
    private string side;

    private GameObject player;
    private PlayerController playerController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }


    private void OnTriggerStay(Collider other)
    {
        CollideWithPlayer collideWithPlayer = null;

        try
        { // try to get collideWithPlayer component; this is to determine if other can colide with player
            collideWithPlayer = other.GetComponent<CollideWithPlayer>();
        }
        catch { }

        if (collideWithPlayer != null)
        { // if other is a collideWithPlayer

            //Debug.Log(side + " stay");
            SetAllowPlayerMovement(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CollideWithPlayer collideWithPlayer = null;

        try
        { // try to get collideWithPlayer component; this is to determine if other can colide with player
            collideWithPlayer = other.GetComponent<CollideWithPlayer>();
        }
        catch { }

        if (collideWithPlayer != null)
        { // if other is a collideWithPlayer

            //Debug.Log(side + " exit");
            SetAllowPlayerMovement(true);
        }

    }

    private void SetAllowPlayerMovement(bool value)
    { // allow or disable player movement in appropriate directaion based on what sensor is this; value determines if movement is going to be set on or off

        switch (side)
        {
            case "front":
                playerController.SetAllowMove("forward", value);
                break;
            case "back":
                playerController.SetAllowMove("back", value);
                break;
            case "left":
                playerController.SetAllowMove("left", value);
                break;
            case "right":
                playerController.SetAllowMove("right", value);
                break;
            default:
                Debug.Log("PlayerSensor: '" + side + "' is not a valid side! Use 'front', 'back', 'left' or 'right'");
                break;
        }
    }

}
