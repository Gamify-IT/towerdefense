using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Handles how when questions appear and evaluates the player's answer
/// </summary>
public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }
    private List<QuestionData> questions;
    private int questionCounter = 0;
    private QuestionData currentQuestion;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private GameObject answerDropdown;
    [SerializeField] private UnityEngine.UI.Button exitButton;
    [SerializeField] public UnityEngine.UI.Button submitButton;
    [SerializeField] private GameObject canvas;

    #region Singelton
    /// <summary>
    ///     Realizes the singelton conecpt, i.e. only one instance can exist at the same time.
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
    public void Start()
    {
        exitButton.onClick.AddListener(() => Quit());
        canvas.SetActive(false);
    }
    
    /// <summary>
    /// Saves the retrieved questions from the backend
    /// </summary>
    /// <param name="questions"></param>
    public void SetQuestions(List<QuestionData> questions)
    {
        this.questions = questions.OrderBy(x => Random.value).ToList();
        this.questions.ForEach(question => Debug.Log(question.GetText()));
    }


    /// <summary>
    /// Loads the current question to the text and dropdown menu of the question scene
    /// </summary>
    public void LoadQuestion()
    {
        Debug.Log("Loading Question...");

        if (questionCounter >= questions.Count)
        {
            Debug.Log("All questions answered");
            // game finished -> display end screen
        }

        currentQuestion = questions[questionCounter];
        FillDropdown();
    }

    /// <summary>
    /// Filles the dropdown entries with the given question
    /// </summary>
    /// <param name="question">question the dropdown is filled with</param>
    private void FillDropdown()
    {
        List<string> dropdownEntries = currentQuestion.GetWrongAnswers().Append(currentQuestion.GetCorrectAnswer()).ToList();
        dropdownEntries = dropdownEntries.OrderBy(x => Random.value).ToList();

        answerDropdown.GetComponent<TMP_Dropdown>().options.Clear();
        questionText.text = currentQuestion.GetText();

        foreach(var answers in dropdownEntries)
        {
            answerDropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(answers));
        }

        questionCounter++;
    }

    /// <summary>
    /// Activates the canvas which shows the question menu
    /// </summary>
    /// <param name="active">visibility of the canvas</param>
    public void ActivateCanvas(bool active)
    {
        canvas.SetActive(active);
    }


    /// <summary>
    /// Checks if the given answer is correct
    /// </summary>
    public bool CheckAnswer()
    {
        Debug.Log("Your Answer: " + answerDropdown.transform.GetChild(0).GetComponent<TMP_Text>().text);
        Debug.Log("Correct Answer: " + currentQuestion.GetCorrectAnswer());

        if (answerDropdown.transform.GetChild(0).GetComponent<TMP_Text>().text == currentQuestion.GetCorrectAnswer())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Quits the minigame, i.e. the player returns to the overworld
    /// </summary>
    private void Quit()
    {
        SceneManager.UnloadSceneAsync("Question");
        UIManager.Instance.SetHoveringState(false);
    }
}
