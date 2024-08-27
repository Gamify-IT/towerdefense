using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
   /// <summary>
   /// Pauses the game and loads the pause menu
   /// </summary>
   /// <param name="sceneID"> the ID of the Pause menu scene</param>
    public void Pause(int sceneID)
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(sceneID, LoadSceneMode.Additive);
    }
}
