using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private string type;

    public bool collideWithPlayer = true;

    public string GetBlockType()
    {
        return type;
    }
}
