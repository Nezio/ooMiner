using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour
{
    
    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadScene(string name)
    {
        try
        {
            SceneManager.LoadScene(name);
        }
        catch
        {
            Debug.Log("Could not load scene: '" + name + "'!");
        }
    }



}
