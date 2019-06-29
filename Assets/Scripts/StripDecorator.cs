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

    public int numberOfBlocksToSpawn;

    
    private List<GameObject> blocks;
    private List<int> freeXPositions; // list of x coordinates that are yet unoccupied by a block

    private void Start()
    {
        blocks = new List<GameObject>();

        // initialize list of free positions
        freeXPositions = new List<int>();
        for(int i = positionMin; i <= positionMax; i++)
            freeXPositions.Add(i);

        InitializeBlocks();
    }

    private void InitializeBlocks()
    {
        for(int i = 0; i < numberOfBlocksToSpawn; i++)
        { // try to spawn numberOfBlocksToSpawn blocks
            // spawn only if there are free positions
            if (freeXPositions.Count != 0)
            {
                // choose next block
                GameObject nextBlockType = ChooseNextBlock();

                // choose x position for the new block
                int freeXPositionsIndex = Random.Range(0, freeXPositions.Count);
                int newXPosition = freeXPositions[freeXPositionsIndex];

                // remove taken position from freePositions list
                freeXPositions.RemoveAt(freeXPositionsIndex);

                // spawn the block
                Vector3 newPosition = new Vector3(newXPosition, 0, transform.position.z);
                GameObject newBlock = GameObject.Instantiate(nextBlockType, newPosition, Quaternion.identity);

                // set parent of the block
                newBlock


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

        return block;
    }

    public void RedecorateStrip()
    { // redecorates this strip; call this upon successful pooling of the strip

    }


}
