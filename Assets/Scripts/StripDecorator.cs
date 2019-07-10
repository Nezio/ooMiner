using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripDecorator : MonoBehaviour
{ // attach this script to the strip prefab to allow it to spawn blocks on top of itself

    public GameObject[] BlockGallery;

    [Tooltip("X Position from which block spawning starts in global world space (inclusive)")]
    public int positionMin;
    [Tooltip("X Position to which block spawning goes in global world space (inclusive)")]
    public int positionMax;

    [SerializeField]
    private int numberOfBlocksToSpawn;   // if you want to change this dynamically you must add a function that will change it and resize blocks array
    public int poolSize = 10;
    
    
    private List<GameObject> blocks;
    private GameObject[] pool;

    private GameManager gameManager;

    private void Start()
    {
        // initialize game manager and get run start time
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        

        // initialize blocks array
        blocks = new List<GameObject>();

        // initialize pool array
        pool = new GameObject[poolSize];

        InitializeBlocks();
    }

    private void Update()
    {
        if(gameManager.GetRunStartTime() > 0)
        { // if run started
            //Debug.Log(Time.time - gameManager.GetRunStartTime());

            // try to set new numberOfBlocksToSpawn (it will succeed if enough time has passed)
            TrySetNewNumberOfBlocksToSpawnAtTime(10, 2);
            TrySetNewNumberOfBlocksToSpawnAtTime(30, 3);
            TrySetNewNumberOfBlocksToSpawnAtTime(70, 4);
            TrySetNewNumberOfBlocksToSpawnAtTime(120, 5);
        }
    }

    private void TrySetNewNumberOfBlocksToSpawnAtTime(float time, int newNumberOfBlocksToSpawn)
    { // time - time from the start of the run when newNumberOfBlocksToSpawn will be set

        float timeFromRunStart = Time.time - gameManager.GetRunStartTime();

        // if blocks array isn't set already and time is right
        if (blocks.Count < newNumberOfBlocksToSpawn && timeFromRunStart > time)
        {
            // set number of blocks to 3
            numberOfBlocksToSpawn = newNumberOfBlocksToSpawn;

            // add missing blocks to blocks array
            for (int i = blocks.Count; i < numberOfBlocksToSpawn; i++)
            {
                // spawn the block (instantiate)
                GameObject newBlock = GameObject.Instantiate(ChooseNextBlock(), new Vector3(-500, 0, transform.position.z), Quaternion.identity);

                // set parent of the block
                newBlock.transform.SetParent(transform);

                // add the block to the list of blocks
                blocks.Add(newBlock);
            }
        }
    }

    private void InitializeBlocks()
    {
        List<int> freeXPositions = new List<int>(); // list of x coordinates that are yet unoccupied by a block
        for(int i = positionMin; i <= positionMax; i++) // initialize list of free positions
            freeXPositions.Add(i);

        for(int i = 0; i < numberOfBlocksToSpawn; i++)
        { // try to spawn numberOfBlocksToSpawn blocks
            // spawn only if there are free positions
            if (freeXPositions.Count != 0)
            {
                // choose next block
                GameObject nextBlockType = ChooseNextBlock();

                // choose next position (also removes taken position from freeXPositions array)
                Vector3 newPosition =  ChooseNextPosition(ref freeXPositions);

                // spawn the block (instantiate)
                GameObject newBlock = GameObject.Instantiate(nextBlockType, newPosition, Quaternion.identity);

                // set parent of the block
                newBlock.transform.SetParent(transform);

                // add the block to the list of blocks
                blocks.Add(newBlock);
            }
            else
                break;  // don't try to spawn any more blocks if there are no free positions left
        }


    }

    private GameObject ChooseNextBlock()
    {
        GameObject block = BlockGallery[Random.Range(0, BlockGallery.Length)];

        // you will probably want to add block spawn chance check here at some point or do code to decrease apparent randomness

        return block;
    }

    private Vector3 ChooseNextPosition(ref List<int> freeXPositions)
    { // choose next Vector3 position taking into account the list of free X positions; NOTE: also removes taken position from freeXPositions array

        // choose x position for the new block from the list of free positions
        int freeXPositionsIndex = Random.Range(0, freeXPositions.Count);
        int newPosition_X = freeXPositions[freeXPositionsIndex];

        // remove taken X position from freeXPositions list
        freeXPositions.RemoveAt(freeXPositionsIndex);

        // set new position as a Vector3
        Vector3 newPosition = new Vector3(newPosition_X, 0, transform.position.z);

        return newPosition;        
    }

    public void RedecorateStrip()
    { // redecorates this strip; call this upon successful pooling of the strip

        // choose n
        int n = numberOfBlocksToSpawn;

        // backfill pool
        Tools.Backfill(ref pool);

        // destroy first n entries of the pool
        Tools.DestroyFirstN(n, ref pool);

        // shift the pool back by n (by how many old block are going to be added to it)
        Tools.ShiftArrayBackByN(n, ref pool);

        // add old blocks to the back of the pool (save first n blocks to the last n pool slots) and push them off-screen
        for (int i = 0; i < n; i++)
        {
            pool[pool.Length - 1 - i] = blocks[i];
            blocks[i].transform.Translate(-500, 0, 0, Space.World);
        }



        List<int> freeXPositions = new List<int>(); // list of x coordinates that are yet unoccupied by a block
        for (int i = positionMin; i <= positionMax; i++) // initialize list of free positions
            freeXPositions.Add(i);

        // fill last blocks list by generating random blocks; look at pool to check if it already exists
        for (int i = 0; i < blocks.Count; i++)
        { // for each block that should be spawned

            // next block object
            GameObject nextBlock = null;
            // choose next block
            GameObject nextBlockType = ChooseNextBlock();
            // choose next position (also removes taken position from freeXPositions array)
            Vector3 newPosition = ChooseNextPosition(ref freeXPositions);

            // check the pool
            for (int j = 0; j < pool.Length; j++)
            {
                if (pool[j] != null && pool[j].GetComponent<Block>().GetBlockType() == nextBlockType.GetComponent<Block>().GetBlockType())
                { // found match
                    nextBlock = pool[j];
                    pool[j] = null;
                    break;
                }
            }
            if (nextBlock == null)
            { // in case there is no matching strip found in the pool instantiate new one
                nextBlock = GameObject.Instantiate(nextBlockType, newPosition, Quaternion.identity);
                //Debug.Log("instantiating new block");
            }
            else
            { // a match was found in the pool
                // move to a new position
                //nextBlock.transform.SetParent(null);    // uncomment if it doesn't move propperly while it has a parent
                nextBlock.transform.position = newPosition;
            }

            // set parent of the block whether instantiated or taken from the pool (if taken from pool nothing will happen)
            nextBlock.transform.SetParent(transform);

            // set reference to this new block
            blocks[i] = nextBlock;

            // in case block is disabled (happens with coins and other collectibles) enable it
            blocks[i].GetComponent<Block>().ReinitializeBlock();
        }
        
    }


}
