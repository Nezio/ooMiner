using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource source;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        //source.pitch = Time.timeScale;
    }
}
