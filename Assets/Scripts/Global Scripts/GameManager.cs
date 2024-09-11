using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

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

    void Start()
    {
        FetchAllQuestions();
    }

    /// <summary>
    ///     Retrieves all questions of the configuration from the backend and saves them in the QuestionManager.
    /// </summary>
    private async void FetchAllQuestions()
    {
    #if !Unity_Editor
        string originURL = GetOriginUrl();
        string baseBackendPath = GameSettings.GetTowerdefenseBackendPath();
        string configurationAsUUID = GetConfiguration();

        string path = originURL + baseBackendPath + "/configurations/" + configurationAsUUID;

        Optional<ConfigurationDTO> configurationDTO = await RestRequest.GetRequest<ConfigurationDTO>(path);

        if (configurationDTO.IsPresent())
        {
            ConfigurationData configuration = ConfigurationData.ConvertDtoToData(configurationDTO.Value());
            QuestionManager.Instance.SetQuestions(configuration.GetQuestions().ToList());
        }
    #endif
    }
}
