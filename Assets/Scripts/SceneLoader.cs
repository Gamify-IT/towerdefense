using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region JavaScript Methods
    [DllImport("__Internal")]
    private static extern void CloseMinigame();
    #endregion

    [Header("Audio Elements")]
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

    [Header("Scene Names")]
    public string mainGameScene = "Level1"; 
    public string playerHUDScene = "PlayerHUD"; 
    public string shopScene = "Shop";

    #region Singelton
    public static SceneLoader Instance { get; private set; }

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
    #endregion

    /// <summary>
    /// Initializes audio source at the begin
    /// </summary>
    public void Start()
    {
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip=clickSound;
    }

    /// <summary>
    /// Loads the main game with all required scenes
    /// </summary>
    public async void StartGame()
    {
        Debug.Log("Starting game...");
        await PlayClickSound();
        SceneManager.LoadSceneAsync(mainGameScene);
        SceneManager.LoadSceneAsync(playerHUDScene, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(shopScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// This function closes the game
    /// </summary>
    public async void Quit()
    {
        Debug.Log("Quitting the game...");
        await PlayClickSound();
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
   
        CloseMinigame();
#endif
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
