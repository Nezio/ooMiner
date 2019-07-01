using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonImageUpdater : MonoBehaviour
{

    public Sprite musicOnImage;
    public Sprite musicOffImage;

    private void Start()
    {
        if(AudioListener.pause)
        { // audio listener is paused = sound off
            SetMusicOffImage();
        }
        else
        { // audio listener is uppaused = sound on
            SetMusicOnImage();
        }
        
    }

    public void SetMusicOnImage()
    {
        gameObject.GetComponent<Image>().sprite = musicOnImage;
    }

    public void SetMusicOffImage()
    {
        gameObject.GetComponent<Image>().sprite = musicOffImage;
    }

}
