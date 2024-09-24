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
    private int volumeLevel;
    #region JavaScript Methods
    [DllImport("__Internal")]
    private static extern string GetConfiguration();

    [DllImport("__Internal")]
    private static extern string GetOriginUrl();
    #endregion

    #region Singelton
    /// <summary>
    ///     Realizes the the singelton conecpt, i.e. only one instance can exist at the same time.
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
        Debug.Log("Skippig due to Unity Editor");
        await SceneManager.LoadSceneAsync("Question", LoadSceneMode.Additive);
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
}