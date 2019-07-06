using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeInput : MonoBehaviour
{
    public Text debugText;

    // If the touch is longer than maxSwipeTime, we don't consider it a swipe
    public const float maxSwipeTime = 0.5f;

    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    public const float minSwipeDistance = 0.02f;

    public const float maxTapTime = 0.3f;
    public const float maxTapDistance = 0.01f; // should be smaller than minSwipeDistance


    public static bool swipedRight = false;
    public static bool swipedLeft = false;
    public static bool swipedUp = false;
    public static bool swipedDown = false;
    public static bool tap = false;

    public bool debugWithArrowKeys = true;

    private Vector2 startPos;
    private float startTime;

    public void Update()
    {
        swipedRight = false;
        swipedLeft = false;
        swipedUp = false;
        swipedDown = false;
        tap = false;

        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);
            bool touchedButton = false;

            if (touch.phase == TouchPhase.Began)
            {
                // set startPos as value between 0 and 1
                startPos = new Vector2(touch.position.x / (float)Screen.width, touch.position.y / (float)Screen.width);
                startTime = Time.time;

                //debugText.text = "Touch began!\n" + debugText.text;

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                { // ui touched
                    if(EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    { // detect tap on button
                        //debugText.text = "Button!\n" + debugText.text;

                        touchedButton = true;
                    }

                    //debugText.text = "Tap on UI!\n" + debugText.text;
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 endPos = new Vector2(touch.position.x / (float)Screen.width, touch.position.y / (float)Screen.width);
                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                // detect a swipe only if not too long and not too short and not a button
                if (Time.time - startTime <= maxSwipeTime && swipe.magnitude >= minSwipeDistance && !touchedButton)
                { // swipe detected
                    //debugText.text = "Swipe!\n" + debugText.text;

                    if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                    { // Horizontal swipe
                        if (swipe.x > 0)
                        {
                            swipedRight = true;
                        }
                        else
                        {
                            swipedLeft = true;
                        }
                    }
                    else
                    { // Vertical swipe
                        if (swipe.y > 0)
                        {
                            swipedUp = true;
                        }
                        else
                        {
                            swipedDown = true;
                        }
                    }
                }
                else if(Time.time - startTime <= maxTapTime && swipe.magnitude < maxTapDistance && !touchedButton)
                { // tap detected
                    //debugText.text = "Tap elswhere!\n" + debugText.text;
                    //debugText.text = "Time scale: " + Time.timeScale + "\n" + debugText.text;

                    tap = true;
                }
                
            }
            if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved && !touchedButton)
            {
                // if touch longer than both max time for swipe and tap
                if(Time.time - startTime > maxTapTime && Time.time - startTime > maxSwipeTime)
                { // hold
                    ;
                }
            }
        }

        if (debugWithArrowKeys)
        {
            swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow);
            swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow);
            swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow);
            swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
        }
    }
}