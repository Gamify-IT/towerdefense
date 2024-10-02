using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
///  This class manages the pause menu including its resume and pause logic
/// </summary>
public class PauseMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject confirmMenu;

    private bool mouseOver = false;

    /// <summary>
    /// Resumes the current game session
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("Pause");
        UIManager.Instance.SetHoveringState(false);
    }

    /// <summary>
    /// Opens the confirm menu if player wants to quit
    /// </summary>
    public void OpenConfirmMenu()
    {
        confirmMenu.SetActive(true);
    }

    /// <summary>
    /// Closes the confirm menu if player wants to continue playing
    /// </summary>
    public void CloseConfirmMenu()
    {
        confirmMenu.SetActive(false);
    }

    /// <summary>
    /// Quits the game and the player returns to the overworld
    /// </summary>
    public void Quit()
    {
        Time.timeScale = 1f;
        CloseConfirmMenu();
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
