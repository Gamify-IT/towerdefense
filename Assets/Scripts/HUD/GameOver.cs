using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public async void RestartGame()
    {
        Debug.Log("Restarting game...");
        await PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Menu");
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
