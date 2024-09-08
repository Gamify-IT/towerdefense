using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
///  This class manages the pause menu inlcuding its resume and pasue logic
/// </summary>
public class PauseMenu : MonoBehaviour
{
    private bool mouseOver = false;

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
