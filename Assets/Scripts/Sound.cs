using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public AudioSource source;

    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1;
    public float pitch = 1;    
    //public bool unscaledTime;   // makes sound unaffected by timescale changes; sounds using scaled time change their pitch when timescale changes but they have to be constantly updated

    public bool loop;
 
}

