using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strip : MonoBehaviour
{
    [Tooltip("Strips in this list can't be spawned after this strip type.")]
    public Strip[] nextStripBlacklist;

    [SerializeField]
    private string type;

    public string GetStripType()
    {
        return type;
    }

    // array of decorations

    // decoration generator

    // redecorator
}
