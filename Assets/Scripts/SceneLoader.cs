using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string mainGameScene = "Level1"; 
    public string playerHUDScene = "PlayerHUD"; 
    public string shopScene = "Shop"; 

    public void Start()
    {
        AudioListener.volume = 0f;
        GameManager.Instance.FetchAllQuestions();
    }

   /// <summary>
   /// Loads the main game
   /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(mainGameScene);
        SceneManager.LoadScene(playerHUDScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(shopScene, LoadSceneMode.Additive);
    }

    public void Quit()
    {
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
