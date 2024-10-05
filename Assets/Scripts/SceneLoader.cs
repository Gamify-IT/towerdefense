using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region JavaScript Methods
    [DllImport("__Internal")]
    private static extern void CloseMinigame();
    #endregion

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
    /// Loads the main game with all required scenes
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadSceneAsync(mainGameScene);
        SceneManager.LoadSceneAsync(playerHUDScene, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(shopScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// This function quits the game and returns the player to the overworld
    /// </summary>
    public void Quit()
    {
        Debug.Log("Quitting the game...");
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
   
        CloseMinigame();
#endif
    }

}
