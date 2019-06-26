using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strip : MonoBehaviour
{
    [Tooltip("This strip can't be spawned after strips added to this array.")]
    public Strip[] cantSpawnAfter;

    [SerializeField]
    private string type;

    public string GetType()
    {
        return type;
    }

    // array of decorations

    // decoration generator

    // redecorator
}
