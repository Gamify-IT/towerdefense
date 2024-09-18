using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }
    private List<QuestionData> questions;
    private int questionCounter = 0;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private GameObject answerDropdown;

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

    public void SetQuestions(List<QuestionData> questions)
    {
        this.questions = questions;
        questions.ForEach(question => Debug.Log(question.GetText()));
    }


    /// <summary>
    /// Loads the current question to the text and dropdown menu of the question scene
    /// </summary>
    private void LoadQuestion()
    {
        QuestionData currentQuestion = questions[questionCounter];
        questionText.text = currentQuestion.GetText();
        answerDropdown.GetComponent<Dropdown>().options.Clear();
        answerDropdown.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(currentQuestion.GetCorrectAnswer()));
        foreach (var question in currentQuestion.GetWrongAnswers()) { answerDropdown.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(question)); }
        questionCounter++;
    }
}
