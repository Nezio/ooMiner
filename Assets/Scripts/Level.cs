using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject startingStrypeType;
    public GameObject[] stripGallery;
    public int stripsArraySize = 40;
    public int stripPoolSize = 30;
    public int generateStripCount = 10;    // number of strips to be generated at a time

    private GameObject[] strips;
    private GameObject[] stripsPool;
    private Camera mainCamera;
    private CameraController cameraController;

    private void Awake()
    {
        // initialize level / spawn initial strips
        strips = new GameObject[stripsArraySize];
        for (int i = 0; i < strips.Length; i++)
        {
            GameObject nextStripType;

            // first few are always the same
            if(i < 15)
                nextStripType = startingStrypeType;
            else
                nextStripType = GenerateNextStrip(strips[i-1]);

            // instantiate selected strip and add to array of strips
            strips[i] = GameObject.Instantiate(nextStripType, transform, false);

            // position the new strip
            strips[i].transform.localPosition = new Vector3(0, -3 + nextStripType.transform.position.y, i);
        }
    }

    private void Start()
    {
        // get main camera and camera controller
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<CameraController>();

        
        // initialize strips pool
        stripsPool = new GameObject[stripPoolSize];


    }

    private void Update()
    {
        // check if new blocks should be generated
        GenerateCheck();
    }

    public void GenerateCheck()
    {
        // just in case strips aren't initialized somehow
        if(strips[0] == null)
            return;

        // how far is the camera from the first strip
        float distanceCameraToFirstStrip = mainCamera.transform.position.z - strips[0].transform.position.z;
        //Debug.Log(distanceCameraToFirstStrip);
        // how many strips are out of view
        int stripsOutOfView = Mathf.FloorToInt(distanceCameraToFirstStrip) - cameraController.cameraBackwardsViewDistance;

        if (stripsOutOfView >= generateStripCount)
            GenerateStrips();

    }

    private GameObject GenerateNextStrip(GameObject previousStrip)
    {
        // make new tmp gallery array that is not going to contain strips that can't be spawned after prevoius strip type


        GameObject strip = stripGallery[Random.Range(0, stripGallery.Length)];

        return strip;
    }

    private void GenerateStrips()
    {
        // note: n is number of strips to be generated; n = generateStripCount

        // reorder stripsPool array so that null values are at the front
        Tools.BackfillArray(ref stripsPool);

        // if there are some non-null entries in the first n slots of stripsPool -> destroy those game objects
        for(int i = 0; i < generateStripCount; i++)
        {
            if (stripsPool[i] != null)
            {
                GameObject.Destroy(stripsPool[i]);
                //Debug.Log("destroyed");
            }
        }

        // shift stripsPool array by n slots back
        Tools.ShiftArrayBackbyN(generateStripCount, ref stripsPool);

        // save first n strips to last n pooling array slots
        for(int i = 0; i < generateStripCount; i++)
        {
            stripsPool[stripsPool.Length - 1 - i] = strips[i];
        }


        // shift strips array by n backwards
        Tools.ShiftArrayBackbyN(generateStripCount, ref strips);

        // fill last n slots of strips array by generating random strips; look at stripsPool to check if already exists
        Vector3 lastStripLocPos = strips[strips.Length - 1].transform.localPosition;
        int newZOffset = 1;     // helper counter; this could also be calculated from i index
        for(int i = strips.Length-generateStripCount; i < strips.Length; i++)
        {
            GameObject nextStrip = null;   // next strip object
            // select random next strip type from refabs
            GameObject nextStripType = GenerateNextStrip(strips[i-1]);
            
            // check the pool
            for (int j = 0; j < stripsPool.Length; j++)
            {
                if(stripsPool[j] != null && stripsPool[j].GetComponent<Strip>().GetType() == nextStripType.GetComponent<Strip>().GetType())
                { // found match
                    nextStrip = stripsPool[j];
                    stripsPool[j] = null;
                    break;
                }
            }
            // in case there is no matching strip found in the pool
            if(nextStrip == null)
                nextStrip = GameObject.Instantiate(nextStripType, transform, false);

            // set position of this new strip
            nextStrip.transform.localPosition = new Vector3(0, -3 + nextStripType.transform.position.y, lastStripLocPos.z + newZOffset);
            // set reference to this new strip
            strips[i] = nextStrip;

            newZOffset++;
        }

    }

    public GameObject[] GetStrips()
    {
        return strips;
    }

    
}
