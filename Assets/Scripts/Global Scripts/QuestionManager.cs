using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }
    private List<QuestionData> questions;
    [SerializeField] private GameObject questionText;
    [SerializeField] private GameObject answerDropdown;
    private string correctAnswerText;
    private string wrongAnswerText;

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

    private void LoadQuestion()
    {

    }
}
