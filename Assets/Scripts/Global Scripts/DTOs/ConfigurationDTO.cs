using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is used to retrieve configuration data from get requests.
/// </summary>
[Serializable]
public class ConfigurationDTO
{
    public QuestionDTO[] questions;
    public int volumeLevel;

    public ConfigurationDTO(QuestionDTO[] questions)
    {
        this.questions = questions;
    }

    /// <summary>
    ///     This function converts a <c>ConfigurationData</c> object into a <c>ConfigurationDTO</c> instance
    /// </summary>
    /// <param name="data">The <c>ConfigurationData</c> object to convert</param>
    /// <returns>The <c>ConfigurationDTO</c> instance</returns>
    public static ConfigurationDTO ConvertDataToDTO(ConfigurationData data)
    {
        QuestionDTO[] questions = new QuestionDTO[data.GetQuestions().Length];

        for (int i = 0; i < questions.Length; i++)
        {
            questions[i] = QuestionDTO.ConvertDataToDTO(data.GetQuestions()[i]);
        }

        return new ConfigurationDTO(questions);
    }

    /// <summary>
    ///     This function converts a json string to a <c>ConfigurationDTO</c> object.
    /// </summary>
    /// <param name="jsonString">The json string to convert</param>
    /// <returns>A <c>ConfigurationDTO</c> object containing the data</returns>
    public static ConfigurationDTO CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ConfigurationDTO>(jsonString);
    }

}