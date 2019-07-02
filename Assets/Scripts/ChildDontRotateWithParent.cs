using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDontRotateWithParent : MonoBehaviour
{ // attach this script to child objects that should not rotate along with parent (eg.: player sensors)

    Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
