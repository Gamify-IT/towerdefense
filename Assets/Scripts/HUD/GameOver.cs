using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  This class is used for managing the game over screen
/// </summary>
public class GameOver : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

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
    /// This function plays the click sound when clicking on the restart button
    /// </summary>
    public void RestartGame()
    {
        PlayClickSound();
        Invoke("CallStartMenuScene", 0.2f);
    }

    /// <summary>
    /// This function calls the start menu scene
    /// </summary>
    private void CallStartMenuScene()
    {
        SceneManager.LoadScene("Start Menu");
    }

    /// <summary>
    /// This function calls the coroutine to play click sound and exit the game
    /// </summary>
    public void ExitGame()
    {
        StartCoroutine(ExitGameAfterClickSound());
    }

    /// <summary>
    /// This function plays the click sound and after that closes the game
    /// </summary>
    private IEnumerator ExitGameAfterClickSound()
    {
        PlayClickSound();
        yield return new WaitForSeconds(0.2f);
        Application.Quit();

        // Game quits, when you are in the Unity editor with the following code line.
        // Relevant while game is in development.
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
