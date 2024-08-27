using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{

    
    public void Quit(int PuaseSceneId)
    {
        Debug.Log("Quitting the game...");
        //quitting action in unity editor for testing
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
   
    Application.Quit();
#endif
        // TO implement: ask if user is sure about this action
    }
}
