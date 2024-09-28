using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
///  This class manages the pause menu including its resume and pause logic
/// </summary>
public class PauseMenu : MonoBehaviour
{
    private bool mouseOver = false;

    /// <summary>
    /// Resumes the current game session
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("Pause");
    }

    /// <summary>
    /// Exits the game and returns to the start menu
    /// </summary>
    public void Exit()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.Quit();
    }

    /// <summary>
    ///  This function sets the setHoveringState function to true if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        UIManager.Instance.SetHoveringState(true);
    }

    /// <summary>
    ///  This function sets the setHoveringState function to false if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        UIManager.Instance.SetHoveringState(false);
    }

}
