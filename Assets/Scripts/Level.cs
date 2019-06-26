using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] stripeGallery;
    public int stripesArraySize = 40;
    public int stripePoolSize = 30;
    public int generateStripeCount = 10;    // number of stripes to be generated at a time


    private GameObject[] stripes;
    private GameObject[] stripesPool;
    private Camera mainCamera;
    private CameraController cameraController;

    private void Awake()
    {
        // initialize level
        stripes = new GameObject[stripesArraySize];
        for (int i = 0; i < stripes.Length; i++)
        {
            GameObject stripe = GameObject.Instantiate(stripeGallery[Random.Range(0, stripeGallery.Length)], transform, false);
            stripe.transform.localPosition = new Vector3(0, -3, i);

            stripes[i] = stripe;
        }
    }

    private void Start()
    {
        // get main camera and camera controller
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<CameraController>();

        
        // initialize stripes pool
        stripesPool = new GameObject[stripePoolSize];


    }

    public void GenerateCheck()
    {
        // just in case stripes aren't initialized somehow
        if(stripes[0] == null)
            return;

        // how far is the camera from the first stripe
        float distanceCameraToFirstStripe = mainCamera.transform.position.z - stripes[0].transform.position.z;
        //Debug.Log(distanceCameraToFirstStripe);
        // how many stripes are out of view
        int stripesOutOfView = Mathf.FloorToInt(distanceCameraToFirstStripe) - cameraController.cameraBackwardsViewDistance;

        if (stripesOutOfView >= generateStripeCount)
            GenerateStripes();

    }

    private void GenerateStripes()
    {
        // shift stripesPool array by n slots back; n is number of stripes to be generated = generateStripeCount
        Tools.ShiftArrayBackbyN(generateStripeCount, ref stripesPool);

        // save first n stripes to last n pooling array slots
        for(int i = 0; i < generateStripeCount; i++)
        {
            stripesPool[stripesPool.Length - 1 - i] = stripes[i];
        }

        // shift stripes array by n backwards
        Tools.ShiftArrayBackbyN(generateStripeCount, ref stripes);

        // fill last n slots of stripes array by generating random stripes; look at stripesPool to check if already exists


    }

    
}
