using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is used to retrieve game result data from get requests.
/// </summary>
[Serializable]
public class GameResultDTO
{
    #region attributes
    public int questionCount;
    public int correctQuestionsCount;
    public int wrongQuestionsCount;
    public int points;
    public List<QuestionResultDTO> correctAnsweredQuestions;
    public List<QuestionResultDTO> wrongAnsweredQuestions;
    public string configurationAsUUID;
    public int score;
    public int rewards;
    public string playerId;
    #endregion

    public GameResultDTO(int questionCount, int correctQuestionsCount, int wrongQuestionsCount, int points, 
        List<QuestionResultDTO> correctAnsweredQuestions, List<QuestionResultDTO> wrongAnsweredQuestions, 
        string configurationAsUUID, int score, int rewards)
    {
        this.questionCount = questionCount;
        this.correctQuestionsCount = correctQuestionsCount;
        this.wrongQuestionsCount = wrongQuestionsCount;
        this.points = points;
        this.correctAnsweredQuestions = correctAnsweredQuestions;
        this.wrongAnsweredQuestions = wrongAnsweredQuestions;
        this.configurationAsUUID = configurationAsUUID;
        this.score = score;
        this.rewards = rewards;
    }

    /// <summary>
    ///     This function converts a <c>GameResultData</c> object into a <c>GameResultDTO</c> instance
    /// </summary>
    /// <param name="dto">The <c>GameResultData</c> object to convert</param>
    /// <returns>The <c>GameResultDTO</c> instance</returns>
    public static GameResultDTO ConvertDataToDTO(GameResultData data)
    {
        int questionCount = data.GetQuestionCount();
        int correctAnswerCount = data.GetCorrectAnswerCount();
        int wrongAnswerCount = data.GetWrongAnswerCount();
        int points = data.GetPoints();
        List<QuestionResultDTO> correctAnsweredQuestions = new List<QuestionResultDTO>();
        List<QuestionResultDTO> wrongAnsweredQuestions = new List<QuestionResultDTO>();
        string configurationAsUUID = data.GetConfigurationAsUUID();
        int score = data.GetScore();
        int rewards = data.GetRewards();

        for (int i = 0; i < data.GetCorrectAnsweredQuestions().Count; i++)
        {
            correctAnsweredQuestions.Add(QuestionResultDTO.ConvertDataToDTO(data.GetCorrectAnsweredQuestions()[i]));
        }

        for (int i = 0; i < data.GetWrongAnsweredQuestions().Count; i++)
        {
            wrongAnsweredQuestions.Add(QuestionResultDTO.ConvertDataToDTO(data.GetWrongAnsweredQuestions()[i]));
        }

        return new GameResultDTO(questionCount, correctAnswerCount, wrongAnswerCount, points, correctAnsweredQuestions,
            wrongAnsweredQuestions, configurationAsUUID, score, rewards);
    }

    /// <summary>
    ///     This function converts a json string to a <c>GameResultDTO</c> object.
    /// </summary>
    /// <param name="jsonString">The json string to convert</param>
    /// <returns>A <c>GameResultDTO</c> object containing the data</returns>
    public static GameResultDTO CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameResultDTO>(jsonString);
    }

}