using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  This class is used for managing the game over screen
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("Audio Elements")]
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

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
    /// This function plays the click sound when clicking on the restart button
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        StartCoroutine(RestartAfterSound());
    }

    /// <summary>
    /// Exits the game so the player returns to the overworld
    /// </summary>
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.Quit();
    }

    /// <summary>
    /// This function plays the click sound
    /// </summary>
    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    /// <summary>
    /// Plays the click sound and resumes the game
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestartAfterSound()
    {
        PlayClickSound();
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Menu");
    }

}
