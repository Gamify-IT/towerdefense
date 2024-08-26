using System;

/// <summary>
///     This class represents a player's answer of a single question.
/// </summary>
public class QuestionResultData
{
    #region attributes
    private string questionUUId;
    private string answer;
    #endregion

    public QuestionResultData(string questionUUId, string answer)
    {
        this.questionUUId = questionUUId;
        this.answer = answer;
    }

    /// <summary>
    ///     This function converts a <c>ConfigurationDTO</c> object into a <c>QuestionResultData</c> instance
    /// </summary>
    /// <param name="dto">The <c>ConfigurationDTO</c> object to convert</param>
    /// <returns>The <c>QuestionResultData</c> instance</returns>
    public static QuestionResultData ConvertDtoToData(QuestionResultDTO dto)
    {
        string questionUUId = dto.questionUUId;
        string answer = dto.answer;

        return new QuestionResultData(questionUUId, answer);
    }

    #region getter and setter
    public string GetQuestionUUId()
    {
        return questionUUId;
    }

    public string GetAnswer()
    {
        return answer;
    }

    public void SetAnswer(string answer)
    {
        this.answer = answer;
    }
    #endregion

}