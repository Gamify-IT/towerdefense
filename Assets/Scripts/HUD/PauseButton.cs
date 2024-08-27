using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    [Header("References")]
    public GameObject normalSpeedButton; 
    public GameObject fastSpeedButton;


    private bool isFast = false;

    /// <summary>
    /// Pauses the game and loads the pause menu
    /// </summary>
    /// <param name="sceneID"> the ID of the Pause menu scene</param>
    public void Pause(int sceneID)
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(sceneID, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Toggles the game speed between normal (1x) and fast (2x)
    /// </summary>
    public void ToggleSpeed()
    {
        if (isFast)
        {
            Time.timeScale = 1f; 
            isFast = false;

            normalSpeedButton.SetActive(true);
            fastSpeedButton.SetActive(false);
        }
        else
        {
            Time.timeScale = 2f; 
            isFast = true;

            normalSpeedButton.SetActive(false);
            fastSpeedButton.SetActive(true);
        }
    }
}
