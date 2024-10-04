using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     This script handles and processes all game related data during a tower defense game session.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region JavaScript Methods
    [DllImport("__Internal")]
    private static extern string GetConfiguration();

    [DllImport("__Internal")]
    private static extern string GetOriginUrl();
    #endregion

    private GameResultData gameResult;
    private bool isFinished = false;
    private int volumeLevel;

    #region Singelton
    /// <summary>
    ///     Realizes the the singelton conecpt, i.e. only one instance can exist at the same time.
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

    public async void Start()
    {
        await FetchAllQuestions();
    }

    /// <summary>
    ///     Retrieves all questions of the configuration from the backend and saves them in the QuestionManager.
    ///     Gets volume level and calls the function that applies the volume level to all audio in the game 
    /// </summary>
    public async UniTask FetchAllQuestions()
    {
#if UNITY_EDITOR
        Debug.Log("Skippig question loading due to Unity Editor");
        await SceneManager.LoadSceneAsync("Question", LoadSceneMode.Additive);

        // dummy questions for unity editor
        QuestionData questionOne = new QuestionData("1", "What is 1+1?", "2", new List<string>() { "1", "42", "99" });
        QuestionData questionTwo = new QuestionData("2", "What is 1*1?", "1", new List<string>() { "3", "22", "999" });

        QuestionManager.Instance.SetQuestions(new List<QuestionData>() { questionOne, questionTwo } );
#else
        string originURL = GetOriginUrl();
        string baseBackendPath = GameSettings.GetTowerdefenseBackendPath();
        string configurationAsUUID = GetConfiguration();

        string path = originURL + baseBackendPath + "/configurations/" + configurationAsUUID + "/volume";

        Optional<ConfigurationDTO> configurationDTO = await RestRequest.GetRequest<ConfigurationDTO>(path);

        if (configurationDTO.IsPresent())
        {
            //UIManager.Instance.SetHoveringState(true);
            await SceneManager.LoadSceneAsync("Question", LoadSceneMode.Additive);
            ConfigurationData configuration = ConfigurationData.ConvertDtoToData(configurationDTO.Value());
            this.volumeLevel = configurationDTO.Value().volumeLevel;
            UpdateVolumeLevel(this.volumeLevel);
            QuestionManager.Instance.SetQuestions(configuration.GetQuestions().ToList());
        }
#endif
    }

    /// <summary>
    ///     Loads the end screen when the game is finished, i.e. all questions have benn answered by the player
    /// </summary>
    public void LoadEndScreen()
    {
        Debug.Log("Loading end screen...");
        SceneManager.LoadSceneAsync("End Screen", LoadSceneMode.Additive);
    }

    /// <summary>
    ///     Saves the progress of ther current game session in the backend
    /// </summary>
    public async UniTask<bool> SaveProgress(GameResultData result)
    {
        GameResultDTO dto = GameResultDTO.ConvertDataToDTO(result);

        string originURL = GetOriginUrl();
        string baseBackendPath = GameSettings.GetTowerdefenseBackendPath();
        string path = originURL + baseBackendPath + "/results";

        string json = JsonUtility.ToJson(dto, true);

        bool successful = await RestRequest.PostRequest(path, json);

        if(successful)
        {
            Debug.Log("Game Result saved successfully");
            return true;
        }
        else
        {
            Debug.Log("Could not save Game Result");
            return false;
        }
    }

    /// <summary>
    /// Updates the volume level and applies the changes to all audio in the game
    /// </summary>
    private void UpdateVolumeLevel(int volumeLevel)
    {
        float volume = 0f;
        switch (volumeLevel)
        {
            case 0:
                volume = 0f;
                break;
            case 1:
                volume = 0.5f;
                break;
            case 2:
                volume = 1f;
                break;
            case 3:
                volume = 2f;
                break;
        }
        AudioListener.volume = volume;
    }

    #region getter and setter
    /// <summary>
    /// Sets the gamer result to the given one
    /// </summary>
    /// <param name="result">game result to be set</param>
    public void SetGameResult(GameResultData result)
    {
        gameResult = result;
    }

    /// <summary>
    /// Gets the game result of the current session
    /// </summary>
    /// <returns>game result of the current session</returns>
    public GameResultData GetGameResult()
    {
        return gameResult;
    }

    /// <summary>
    /// Sets the curent game status, i.e. if it is finished or not
    /// </summary>
    /// <param name="status"></param>
    public void SetIsFinished(bool status)
    {
        isFinished = status;
    }

    /// <summary>
    /// Gets the current game status, i.e. if it is finished or not
    /// </summary>
    /// <returns></returns>
    public bool IsFinished()
    {
        return isFinished;
    }
    #endregion
}