using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Manages UI elements of the end screen
/// </summary>
public class EndScreen : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0f;       
    }

    /// <summary>
    ///     Quits the game and the player returns to the overworld.
    /// </summary>
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.Quit();
    }

    /// <summary>
    ///     Enables player to start the endless and to continue playing 
    /// </summary>
    public void StartEnlessMode()
    {
        //TODO
    }

}
