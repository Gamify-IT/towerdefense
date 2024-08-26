using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This contains the result of a minigame session.
/// </summary>
public class GameResultData
{
    #region attributes
    private int questionCount;
    private int correctAnswerCount;
    private int wrongAnswerCount;
    private int points;
    private List<QuestionResultData> correctAnsweredQuestions;
    private List<QuestionResultData> wrongAnsweredQuestions;
    private string configurationAsUUID;
    private int score;
    private int rewards;
    private string playerId;
    #endregion

    public GameResultData(int questionCount, int correctAnswerCount, int wrongAnswerCount, int points,
        List<QuestionResultData> correctAnsweredQuestions, List<QuestionResultData> wrongAnsweredQuestions,
        string configurationAsUUID, int score, int rewards, string playerId)
    {
        this.questionCount = questionCount;
        this.correctAnswerCount = correctAnswerCount;
        this.wrongAnswerCount = wrongAnswerCount;
        this.points = points;
        this.correctAnsweredQuestions = correctAnsweredQuestions;
        this.wrongAnsweredQuestions = wrongAnsweredQuestions;
        this.configurationAsUUID = configurationAsUUID;
        this.score = score;
        this.rewards = rewards;
        this.playerId = playerId;
    }

    /// <summary>
    ///     This function converts a <c>GameResultDTO</c> object into a <c>GameResultData</c> instance
    /// </summary>
    /// <param name="dto">The <c>GameResultDTO</c> object to convert</param>
    /// <returns>The <c>GameResultData</c> instance</returns>
    public static GameResultData ConvertDtoToData(GameResultDTO dto)
    {
        int questionCount = dto.questionCount;
        int correctAnswerCount = dto.correctAnswerCount;
        int wrongAnswerCount = dto.wrongAnswerCount;
        int points = dto.points;
        List<QuestionResultData> correctAnsweredQuestions = new List<QuestionResultData>();
        List<QuestionResultData> wrongAnsweredQuestions = new List<QuestionResultData>();
        string configurationAsUUID = dto.configurationAsUUID;
        int score = dto.score;
        int rewards = dto.rewards;
        string playerId = dto.playerId;

        for(int i = 0; i < dto.correctAnsweredQuestions.Count; i++)
        {
            correctAnsweredQuestions[i] = QuestionResultData.ConvertDtoToData(dto.correctAnsweredQuestions[i]);
        }

        for (int i = 0; i < dto.wrongAnsweredQuestions.Count; i++)
        {
            wrongAnsweredQuestions[i] = QuestionResultData.ConvertDtoToData(dto.wrongAnsweredQuestions[i]);
        }

        return new GameResultData(questionCount, correctAnswerCount, wrongAnswerCount, points, correctAnsweredQuestions,
            wrongAnsweredQuestions, configurationAsUUID, score, rewards, playerId);
    }

    #region getter and setter
    public int GetQuestionCount()
    {
        return questionCount;
    }

    public void SetQuestionCount(int questionCount)
    {
        this.questionCount = questionCount;
    }

    public int GetCorrectAnswerCount()
    {
        return correctAnswerCount;
    }

    public void SetCorrectAnswerCount(int correctAnswerCount)
    {
        this.correctAnswerCount = correctAnswerCount;
    }

    public int GetWrongAnswerCount()
    {
        return wrongAnswerCount;
    }

    public void SetWrongAnswerCount(int wrongAnswerCount)
    {
        this.wrongAnswerCount = wrongAnswerCount;
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetPoints(int points)
    {
        this.points = points;
    }

    public List<QuestionResultData> GetCorrectAnsweredQuestions()
    {
        return correctAnsweredQuestions;
    }

    public void SetCorrectAnsweredQuestions(List<QuestionResultData> correctAnsweredQuestions)
    {
        this.correctAnsweredQuestions = correctAnsweredQuestions;
    }

    public List<QuestionResultData> GetWrongAnsweredQuestions()
    {
        return wrongAnsweredQuestions;
    }

    public void SetWrongAnsweredQuestions(List<QuestionResultData> wrongAnsweredQuestions)
    {
        this.wrongAnsweredQuestions = wrongAnsweredQuestions;
    }

    public string GetConfigurationAsUUID()
    {
        return configurationAsUUID;
    }

    public void SetConfigurationAsUUID(string configurationAsUUID)
    {
        this.configurationAsUUID = configurationAsUUID;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    public int GetRewards()
    {
        return rewards;
    }

    public void SetRewards(int rewards)
    {
        this.rewards = rewards;
    }

    public string GetPlayerId()
    {
        return playerId;
    }

    public void SetPlayerId(string playerId)
    {
        this.playerId = playerId;
    }
    #endregion
}