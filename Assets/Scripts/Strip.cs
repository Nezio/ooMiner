using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strip : MonoBehaviour
{
    [Tooltip("Strips in this list can't be spawned after this strip type.")]
    public Strip[] nextStripBlacklist;

    [SerializeField]
    [Tooltip("You probably want this to be different for every variation of a stripe. Same types won't differentiated between while pooling and blacklisting.")]
    private string type;

    public string GetStripType()
    {
        return type;
    }

    // array of decorations ?

    // decoration generator ?

    // redecorator ?
}
