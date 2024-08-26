using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is used to retrieve question data from get requests.
/// </summary>
[Serializable]
public class QuestionDTO
{
    #region attributes
    public string id;
    public string text;
    public string correctAnswer;
    public List<string> wrongAnswers;
    #endregion

    #region constructors
    public QuestionDTO(string id, string text, string correctAnswer, List<string> wrongAnswers)
    {
        this.id = id;
        this.text = text;
        this.correctAnswer = correctAnswer;
        this.wrongAnswers = wrongAnswers;
    }

    public QuestionDTO()
    {
        id = "";
        text = "";
        correctAnswer = "";
        wrongAnswers = new List<string>();
    }
    #endregion

    /// <summary>
    ///     This function converts a <c>QuestionData</c> object into a <c>QuestionDTO</c> instance
    /// </summary>
    /// <param name="data">The <c>QuestionData</c> object to convert</param>
    /// <returns>The <c>QuestionDTO</c> instance</returns>
    public static QuestionDTO ConvertDataToDTO(QuestionData data)
    {
        string id = data.GetId();
        string text = data.GetText();
        string correctAnswer = data.GetCorrectAnswer();
        List<string> wrongAnswers = data.GetWrongAnswers();

        return new QuestionDTO(id, text, correctAnswer, wrongAnswers);
    }

    /// <summary>
    ///     This function converts a json string to a <c>QuestionDTO</c> object.
    /// </summary>
    /// <param name="jsonString">The json string to convert</param>
    /// <returns>A <c>QuestionDTO</c> object containing the data</returns>
    public static QuestionDTO CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QuestionDTO>(jsonString);
    }

}