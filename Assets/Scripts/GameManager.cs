using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // set time scale in case it is frozen
        Time.timeScale = 1f;
    }
}
