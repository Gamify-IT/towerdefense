using System;
using System.Collections.Generic;

/// <summary>
///     This class represents a single question that player can answer.
/// </summary>
[Serializable]
public class QuestionData
{
    #region attributes
    private string id;
    private string text;
    private string correctAnswer;
    private List<string> wrongAnswers;
    #endregion

    public QuestionData(string id, string text, string correctAnswer, List<string> wrongAnswers)
    {
        this.id = id;
        this.text = text;
        this.correctAnswer = correctAnswer;
        this.wrongAnswers = wrongAnswers;
    }

    /// <summary>
    ///     This function converts a <c>QuestionDTO</c> object into a <c>QuestionData</c> instance
    /// </summary>
    /// <param name="dto">The <c>QuestionDTO</c> object to convert</param>
    /// <returns>The <c>QuestionData</c> instance</returns>
    public static QuestionData ConvertDtoToData(QuestionDTO dto)
    {
        string id = dto.id;
        string text = dto.text;
        string correctAnswer = dto.correctAnswer;
        List<string> wrongAnswers = dto.wrongAnswers;

        return new QuestionData(id, text, correctAnswer, wrongAnswers);
    }

    #region getter and setter
    public string GetId()
    {
        return id;
    }

    public void SetId(string id)
    {
        this.id = id;
    }

    public string GetText()
    {
        return text;
    }

    public void SetText(string text)
    {
        this.text = text;
    }

    public string GetCorrectAnswer()
    {
        return correctAnswer;
    }

    public void SetCorrectAnswer(string correctAnswer)
    {
        this.correctAnswer = correctAnswer;
    }

    public List<string> GetWrongAnswers()
    {
        return wrongAnswers;
    }

    public void SetWrongAnswers(List<string> wrongAnswers)
    {
        this.wrongAnswers = wrongAnswers;
    }
    #endregion

}