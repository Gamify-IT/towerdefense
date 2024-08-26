using System;
using UnityEngine;

/// <summary>
///     This class is used to retrieve question result data from get requests.
/// </summary>
[Serializable]
public class QuestionResultDTO
{
    #region attributes
    public string questionUUId;
    public string answer;
    #endregion

    public QuestionResultDTO(string questionUUId, string answer)
    {
        this.questionUUId = questionUUId;
        this.answer = answer;
    }

    /// <summary>
    ///     This function converts a <c>QuestionResultData</c> object into a <c>QuestionResultDTO</c> instance
    /// </summary>
    /// <param name="dto">The <c>QuestionResultData</c> object to convert</param>
    /// <returns>The <c>QuestionResultDTO</c> instance</returns>
    public static QuestionResultDTO ConvertDataToDTO(QuestionResultData data)
    {
        string questionUUId = data.GetQuestionUUId();
        string answer = data.GetAnswer();

        return new QuestionResultDTO(questionUUId, answer);
    }

    /// <summary>
    ///     This function converts a json string to a <c>QuestionResultDTO</c> object.
    /// </summary>
    /// <param name="jsonString">The json string to convert</param>
    /// <returns>A <c>QuestionResultDTO</c> object containing the data</returns>
    public static QuestionResultDTO CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QuestionResultDTO>(jsonString);
    }

}