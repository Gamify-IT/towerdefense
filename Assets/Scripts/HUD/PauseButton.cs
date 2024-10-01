using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
///  This class handles all logic for the pause button
/// </summary>
public class PauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static PauseButton Instance { get; private set; }

    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    [Header("References")]
    public Button speedButton;
    public Sprite normalSpeedSprite;
    public Sprite fastSpeedSprite;
    [SerializeField] GameObject feedbackWindow;

    private bool isFast = false;
    private bool mouseOver = false;

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
    /// Initializes audio source at the begin
    /// </summary>
    void Start()
    {
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip=clickSound;
    }

    /// <summary>
    /// Pauses the game and loads the pause menu
    /// </summary>
    /// <param name="sceneID"> the ID of the Pause menu scene</param>
    public void Pause(int sceneID)
    {
        PlayClickSound();
        Time.timeScale = 0f;
        SceneManager.LoadScene(sceneID, LoadSceneMode.Additive);
    }

    /// <summary>
    /// This function plays the click sound
    /// </summary>
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    /// <summary>
    /// Toggles the game speed between normal (1x) and fast (2x)
    /// </summary>
    public void ToggleSpeed()
    {
        PlayClickSound();
        if (isFast)
        {
            Time.timeScale = 1f; 
            isFast = false;

            speedButton.image.sprite = fastSpeedSprite;
        }
        else
        {
            Time.timeScale = 2f; 
            isFast = true;

            speedButton.image.sprite = normalSpeedSprite;
        }
    }

    /// <summary>
    /// Shows an feedback window with a given text
    /// </summary>
    /// <param name="feedbackText">feedback text to be displayed</param>
    /// <returns></returns>
    public IEnumerator ShowFeedbackWindow(string feedbackText)
    {
        Debug.Log("Show feedback:" + feedbackText);
        feedbackWindow.transform.Find("Feedback Text").GetComponent<TMP_Text>().text = feedbackText;

        feedbackWindow.SetActive(true);
        yield return new WaitForSeconds(2f);
        feedbackWindow.SetActive(false);
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
