using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{ // DEPRECATED
    public float maxMoveOffset = 3f;
    public float moveStep = 0.1f;

    Vector3 startingPosition;
    int moveSide = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // move platform left or right
        transform.position = new Vector3(transform.position.x + moveStep * Time.deltaTime * moveSide, transform.position.y, transform.position.z);

        // change side if max offset is reached
        float offset = Mathf.Abs(startingPosition.x - transform.position.x);
        if (offset > maxMoveOffset)
            moveSide *= -1;
        
    }
}
