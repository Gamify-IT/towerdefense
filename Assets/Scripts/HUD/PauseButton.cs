using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
   
    public void Pause(int sceneID)
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(sceneID, LoadSceneMode.Additive);
    }
}
