using System;

/// <summary>
///     This is the equivalent class to the Configuration.java class in the backend.
///     It contains the configuration of questions in the minigame.
/// </summary>
public class ConfigurationData
{
    private QuestionData[] questions;

    public ConfigurationData(QuestionData[] questions)
    {
        this.questions = questions;
    }

    /// <summary>
    ///     This function converts a <c>ConfigurationDTO</c> object into a <c>ConfigurationData</c> instance
    /// </summary>
    /// <param name="dto">The <c>ConfigurationDTO</c> object to convert</param>
    /// <returns>The <c>ConfigurationData</c> instance</returns>
    public static ConfigurationData ConvertDtoToData(ConfigurationDTO dto)
    {
        QuestionData[] questions = new QuestionData[dto.questions.Length];

        for (int i = 0; i < questions.Length; i++)
        {
            questions[i] = QuestionData.ConvertDtoToData(dto.questions[i]);
        }

        return new ConfigurationData(questions);
    }

    public QuestionData[] GetQuestions()
    {
        return questions;
    }

    public void SetQuestions(QuestionData[] questions)
    {
        this.questions = questions;
    }

}
