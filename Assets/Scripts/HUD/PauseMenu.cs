using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public void Resume(int PuaseSceneId)
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(PuaseSceneId);
    }

    
    public void Quit(int SceneId)
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(SceneId);
    }
}
