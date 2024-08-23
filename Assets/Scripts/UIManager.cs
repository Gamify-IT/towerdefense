using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to handle all the important functions of the UI elements in the game
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private bool isHoveringUI;

    /// <summary>
    /// This function manages the singleton instance, so it initializes the <c>instance</c> variable, if not set, or
    /// deletes the object otherwise
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    ///  This function detects whether the mouse is currently hovering over an UI element
    /// </summary>
    /// <param name="state"> The state of where the mouse is at the moment</param>
    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    /// <summary>
    ///  This function returns  wheater the mouse is hovering over an UI element 
    /// </summary>
    /// <returns>True, if the mouse is hovering over an UI, false otherwise </returns>
    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }
}
