using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  This class is used for managing the game over screen
/// </summary>
public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void ExitGame()
    {
        Application.Quit();

        // Game quits, when you are in the Unity editor with the following code line.
        // Relevant while game is in development.
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
