using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Handles how when questions appear and evaluates the player's answer
/// </summary>
public class QuestionManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region JavaScript Methods
    [DllImport("__Internal")]
    private static extern string GetConfiguration();

    [DllImport("__Internal")]
    private static extern string GetOriginUrl();
    #endregion

    [Header("Question Progress")]
    private List<QuestionData> questions;
    private List<QuestionResultData> correctAnsweredQuestions = new List<QuestionResultData>();
    private List<QuestionResultData> wrongAnsweredQuestions = new List<QuestionResultData>();
    private QuestionData currentQuestion;
    private int questionCounter = 0;
    private int points = 0;
    private int score = 0;
    private int rewards = 0;

    [Header("UI elements")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private GameObject answerDropdown;
    [SerializeField] private GameObject questionMenu;
    [SerializeField] private GameObject question;
    [SerializeField] private GameObject correctAnswer;
    [SerializeField] private GameObject wrongAnswer;
    [SerializeField] private UnityEngine.UI.Button exitButton;
    [SerializeField] public UnityEngine.UI.Button submitButton;

    #region Singelton
    public static QuestionManager Instance { get; private set; }

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
        submitButton.onClick.AddListener(() => CheckAnswer());
        questionMenu.SetActive(false);
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
    public async void LoadQuestion()
    {
        Debug.Log("Loading Question...");        
        currentQuestion = questions[questionCounter];
        FillDropdown();
    }

    /// <summary>
    /// Filles the dropdown entries with the given question
    /// </summary>
    /// <param name="question">question the dropdown is filled with</param>
    private void FillDropdown()
    {
        question.SetActive(true);
        List<string> dropdownEntries = currentQuestion.GetWrongAnswers().Append(currentQuestion.GetCorrectAnswer()).ToList();
        dropdownEntries = dropdownEntries.OrderBy(x => Random.value).ToList();

        answerDropdown.GetComponent<TMP_Dropdown>().captionText.text = "";
        answerDropdown.GetComponent<TMP_Dropdown>().options.Clear();

        questionText.text = currentQuestion.GetText();

        foreach(var answers in dropdownEntries)
        {
            answerDropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(answers));
        }

        answerDropdown.GetComponent<TMP_Dropdown>().value = 0;

        questionCounter++;

        Debug.Log("Question successfully loaded");
    }

    /// <summary>
    /// Activates the canvas which shows the question menu
    /// </summary>
    /// <param name="active">visibility of the canvas</param>
    public void ActivateCanvas(bool active)
    {
        questionMenu.SetActive(active);
    }


    /// <summary>
    /// Checks if the given answer is correct
    /// </summary>
    public bool CheckAnswer()
    {
        question.SetActive(false);

        string answer = answerDropdown.transform.GetChild(0).GetComponent<TMP_Text>().text;

        Debug.Log("Your Answer: " + answer);
        Debug.Log("Correct Answer: " + currentQuestion.GetCorrectAnswer());

        if (answer == currentQuestion.GetCorrectAnswer())
        {
            correctAnswer.SetActive(true);
            AddCorrectAnswerToResult(currentQuestion, answer);

            return true;
        }
        else
        {
            wrongAnswer.SetActive(true);
            AddWrongAnswerToResult(currentQuestion, answer);

            return false;
        }
    }

    /// <summary>
    /// Checks if the game is finished, i.e all questions have been answered by the player
    /// </summary>
    private async void CheckForEnd()
    {
        if (questionCounter >= questions.Count)
        {
            Debug.Log("All questions answered");
            GameManager.Instance.LoadEndScreen();

#if !UNITY_EDITOR
            GameResultData result = new GameResultData(questions.Count, correctAnsweredQuestions.Count, wrongAnsweredQuestions.Count, points,
                correctAnsweredQuestions, wrongAnsweredQuestions, GetConfiguration(), score, rewards);

            bool succesfull = await GameManager.Instance.SaveProgress(result);
#endif
        }
    }

    /// <summary>
    /// This method adds the answered question to the correctly answered questions
    /// <param name="answer">The players answer</param>
    /// </summary>
    private void AddCorrectAnswerToResult(QuestionData question, string answer)
    {
        Debug.Log("Add correct answer to game result: " + answer);
        QuestionResultData correctResult = new QuestionResultData(question, answer);
        correctAnsweredQuestions.Add(correctResult);
    }

    /// <summary>
    /// This method adds the answered question to the wrong answered questions
    /// <param name="answer">The players answer</param>
    /// </summary>
    private void AddWrongAnswerToResult(QuestionData question, string answer)
    {
        Debug.Log("Add wrong answer to game result: " + answer);
        QuestionResultData wrongResult = new QuestionResultData(question, answer);
        wrongAnsweredQuestions.Add(wrongResult);
    }

    /// <summary>
    /// Closes the Question UI menu
    /// </summary>
    public IEnumerator CloseQuestionUI()
    {
        UIManager.Instance.SetHoveringState(false);
        correctAnswer.SetActive(false);
        wrongAnswer.SetActive(false);
        ActivateCanvas(false);

        yield return new WaitForSeconds(1f);
        CheckForEnd();
    }

    /// <summary>
    /// Opens the Question UI menu
    /// </summary>
    public void OpenQuestionUI()
    {
        UIManager.Instance.SetHoveringState(true);
        ActivateCanvas(true);
    }

    /// <summary>
    /// Quits the minigame, i.e. the player returns to the overworld
    /// </summary>
    private void Quit()
    {
        SceneManager.UnloadSceneAsync("Question");
        UIManager.Instance.SetHoveringState(false);
    }

    /// <summary>
    ///  This function sets the setHoveringState function to true if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.SetHoveringState(true);
    }

    /// <summary>
    ///  This function sets the setHoveringState function to false if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.SetHoveringState(false);
    }
}
