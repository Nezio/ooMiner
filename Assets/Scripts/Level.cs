using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Tooltip("Type of the strip that this level begins with.")]
    public GameObject startingStripType;
    [Tooltip("How many strips of starting type will spawn before random generation kicks in.")]
    public int startingAreaSize = 15;
    [Tooltip("Define all the different types of strips that can spawn.")]
    public GameObject[] stripsGallery;
    [Tooltip("How many strips make up this level.")]
    public int stripsArraySize = 40;
    [Tooltip("Size of the pool used for pooling the strips. Lower values mean less objects to choose from when generating next part of the level, which may cause more frequent Instantiate and Destroy calls. Bigger values mean greater memory usage, but lesser processing power usage.")]
    public int stripPoolSize = 30;
    [Tooltip("Number of strips to generate at a time. Also defines how my stips will go off-screen before new set is generated. Increase this value if level seems to pop-in.")]
    public int generateStripCount = 10;    // number of strips to be generated at a time

    private GameObject[] strips;
    private GameObject[] stripsPool;

    private Camera mainCamera;
    private CameraController cameraController;
    private const float stripYOffset = -3f;   // vertical offset for a strip. NOTE: affects all strips, consider changing strip height in a prefab instead

    private void Awake()
    {
        // initialize level / spawn initial strips
        strips = new GameObject[stripsArraySize];
        for (int i = 0; i < strips.Length; i++)
        {
            GameObject nextStripType;

            // first few are always the same
            if(i < startingAreaSize)
                nextStripType = startingStripType;
            else
                nextStripType = ChooseNextStrip(strips[i-1]);

            // instantiate selected strip and add to array of strips
            strips[i] = GameObject.Instantiate(nextStripType, transform, false);

            // position the new strip
            strips[i].transform.localPosition = new Vector3(0, stripYOffset + nextStripType.transform.position.y, i);
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
        int stripsOutOfView = Mathf.FloorToInt(distanceCameraToFirstStrip) - cameraController.backwardsViewDistance;

        // generate new strips if there are more strips out of view than number of strips that can spawn at a time
        if (stripsOutOfView >= generateStripCount)
            GenerateStrips();

    }

    private GameObject ChooseNextStrip(GameObject previousStrip)
    { // choose next strip to spawn taking in consideration previous strip
        // get strips that can't be spawned next
        Strip[] blacklist = previousStrip.GetComponent<Strip>().nextStripBlacklist;

        // make new tmp strip gallery list that is not going to contain strips that can't be spawned after previous strip type
        List<GameObject> allowedStripsGallery = new List<GameObject>();
        for (int i = 0; i < stripsGallery.Length; i++)
        { // for each element that could be spawned...
            bool blacklisted = false;
            for(int j = 0; j < blacklist.Length; j++)
            { // ..check if it is blacklisted trough nextStripBlacklist array
                if(stripsGallery[i].GetComponent<Strip>().GetStripType() == blacklist[j].GetStripType())
                {
                    blacklisted = true;
                    break;
                }
            }
            // if not blacklisted add it to allowdStipsGallery list
            if (!blacklisted)
                allowedStripsGallery.Add(stripsGallery[i]);
        }

        // choose next strip from the list of allowed strips
        GameObject strip = allowedStripsGallery[Random.Range(0, allowedStripsGallery.Count)];

        return strip;
    }

    private void GenerateStrips()
    {
        // note: n is number of strips to be generated; n = generateStripCount

        // reorder stripsPool array so that null values are at the front
        Tools.Backfill(ref stripsPool);

        // if there are some non-null entries in the first n slots of stripsPool -> destroy those game objects
        Tools.DestroyFirstN(generateStripCount, ref stripsPool);

        // shift stripsPool array by n slots back to free up slots at the end for new entries
        Tools.ShiftArrayBackByN(generateStripCount, ref stripsPool);

        // save first n strips to last n pooling array slots
        for(int i = 0; i < generateStripCount; i++)
        {
            stripsPool[stripsPool.Length - 1 - i] = strips[i];
        }
        


        // shift strips array by n backwards
        Tools.ShiftArrayBackByN(generateStripCount, ref strips);

        // fill last n slots of strips array by generating random strips; look at stripsPool to check if strip already exists
        Vector3 lastStripLocPos = strips[strips.Length - 1].transform.localPosition;
        int newZOffset = 1;     // helper counter; this could also be calculated from i index
        for(int i = strips.Length-generateStripCount; i < strips.Length; i++)
        {
            GameObject nextStrip = null;   // next strip object
            // select random next strip type from prefabs
            GameObject nextStripType = ChooseNextStrip(strips[i-1]);
            
            // check the pool
            for (int j = 0; j < stripsPool.Length; j++)
            {
                if(stripsPool[j] != null && stripsPool[j].GetComponent<Strip>().GetStripType() == nextStripType.GetComponent<Strip>().GetStripType())
                { // found match
                    nextStrip = stripsPool[j];
                    stripsPool[j] = null;
                    break;
                }
            }
            
            if (nextStrip == null)
            { // in case there is no matching strip found in the pool instantiate a new one (with propper pool array size this should happen very rarely)
                nextStrip = GameObject.Instantiate(nextStripType, transform, false);
            }  
            else
            { // if there was a match in the pool
                // redecorate strip if it can be redecorated
                try
                {
                    nextStrip.GetComponent<StripDecorator>().RedecorateStrip();
                }
                catch { /* strip is not of the type that has decorations on itself; do nothing */ };
                
            }
            
            // set position of this new strip
            nextStrip.transform.localPosition = new Vector3(0, stripYOffset + nextStripType.transform.position.y, lastStripLocPos.z + newZOffset);
            // set reference to this new strip
            strips[i] = nextStrip;

            newZOffset++;   // increment helper counter
        }

    }

    public GameObject[] GetStrips()
    {
        return strips;
    }

    
}
