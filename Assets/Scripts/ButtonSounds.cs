using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour
{
    private void Start()
    {
        // add new event trigger to the button
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry;   // declare a new entry

        // add event trigger entry for click
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { PointerClick(); });
        eventTrigger.triggers.Add(entry);

        // add event trigger entry for pointer enter
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { PointerEnter(); });
        eventTrigger.triggers.Add(entry);
    }
    
    private void PointerClick()
    {
        AudioManager.instance.PlayOneShot("ButtonClick");
    }

    private void PointerEnter()
    {
        //AudioManager.instance.PlayOneShot("ButtonHover");
    }

}
