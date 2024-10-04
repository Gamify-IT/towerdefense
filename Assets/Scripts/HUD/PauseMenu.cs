using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
///  This class manages the pause menu including its resume and pause logic
/// </summary>
public class PauseMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Audio Elements")]
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

    [Header("UI Elements")]
    [SerializeField] private GameObject confirmMenu;

    private bool mouseOver = false;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = clickSound;
    }

    /// <summary>
    /// Resumes the current game session
    /// </summary>
    public async void Resume()
    {
        await PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("Pause");
        UIManager.Instance.SetHoveringState(false);
    }

    /// <summary>
    /// (De)activates the confirmation menu if the player wants to quit/continue playing
    /// </summary>
    /// <param name="status"></param>
    public void ActivateConfirmMenu(bool status)
    {
        PlayClickSound();
        confirmMenu.SetActive(status);
    }

    /// <summary>
    /// Quits the game and the player returns to the overworld
    /// </summary>
    public void Quit()
    {
        Time.timeScale = 1f;
        ActivateConfirmMenu(false);
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

    /// <summary>
    /// This function plays the click sound
    /// </summary>
    private async UniTask<bool> PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
            await Task.Delay(100);
        }
        return true;
    }

}
