using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    public string mainGameScene = "Level1"; 
    public string playerHUDScene = "PlayerHUD"; 
    public string shopScene = "Shop"; 

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
        AudioListener.volume = 0f;
        GameManager.Instance.FetchAllQuestions();
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
    /// Loads the main game
    /// </summary>
    public void StartGame()
    {
        PlayClickSound();
        SceneManager.LoadScene(mainGameScene);
        SceneManager.LoadScene(playerHUDScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(shopScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// This function calls coroutine for function that calls click sound and then close the game
    /// </summary>
    public void Quit()
    {
        StartCoroutine(QuitAfterSound());
    }

    /// <summary>
    /// This function calls click sound and then close the game
    /// </summary>
    private IEnumerator QuitAfterSound()
    {
        PlayClickSound();
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Quitting the game...");
        //quitting action in unity editor for testing
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
   
    Application.Quit();
#endif
        // TO implement: ask if user is sure about this action
    }

}
