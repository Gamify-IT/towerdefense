using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  This class is used to handle all the important functions of the UI elements in the game
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private bool isHoveringUI = true;

    /// <summary>
    /// This function manages the singleton instance, so it initializes the <c>instance</c> variable, if not set, or
    /// deletes the object otherwise
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unloads all HUD related and menu scenes.
    /// </summary>
    public void UnloadHUD()
    {
        SceneManager.UnloadScene("Shop");

    }

    /// <summary>
    ///  This function detects whether the mouse is currently hovering over an UI element
    /// </summary>
    /// <param name="state">The state whether the player hovers over an UI element</param>
    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    /// <summary>
    ///  This function returns  whether the mouse is hovering over an UI element 
    /// </summary>
    /// <returns>True, if the mouse is hovering over an UI, false otherwise </returns>
    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }
}
