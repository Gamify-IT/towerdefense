using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
///     Manages UI elements of the end screen
/// </summary>
public class EndScreen : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI elements")]
    [SerializeField] private TMP_Text rewardsText;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject resultScreen;

    [Header("Game data")]
    private GameResultData result;

    private void Start()
    {
        Time.timeScale = 0f;   
        result = GameManager.Instance.GetGameResult();
        rewardsText.text = result.GetScore().ToString() + "  " + "scores" + "  " + "and" + "  " + result.GetRewards().ToString() + "  " + "coins";
    }

    #region button methods
    /// <summary>
    ///     Displays the game results of the current session on the end screen
    /// </summary>
    public void ShowResult()
    {
        ActivateResultScreen(true);
    }

    /// <summary>
    /// (De)activates the resuts screen
    /// </summary>
    /// <param name="status">status whether the result screen shiuld be active or not</param>
    public void ActivateResultScreen(bool status)
    {
        resultScreen.SetActive(status);
        endScreen.SetActive(!status);
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
    ///     Enables player to start the endless mode and to continue playing 
    /// </summary>
    public void StartEnlessMode()
    {
        //TODO
    }
    #endregion

    /// <summary>
    ///  This function sets the setHoveringState function to true if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.SetHoveringState(true);
    }

    /// <summary>
    ///  This function sets the setHoveringState function to false if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.SetHoveringState(false);
    }
}
