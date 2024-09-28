using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Header("Attributes")]
    private List<QuestionData> questions;
    private int questionCounter = 0;
    private QuestionData currentQuestion;

    [Header("UI elements")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private GameObject answerDropdown;
    [SerializeField] private UnityEngine.UI.Button exitButton;
    [SerializeField] public UnityEngine.UI.Button submitButton;
    [SerializeField] private GameObject questionMenu;
    [SerializeField] private GameObject question;
    [SerializeField] private GameObject rightAnswer;
    [SerializeField] private GameObject wrongAnswer;

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
    public void LoadQuestion()
    {
        Debug.Log("Loading Question...");

        if (questionCounter >= questions.Count)
        {
            Debug.Log("All questions answered");
            // TODO game finished -> display end screen
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

        Debug.Log("Your Answer: " + answerDropdown.transform.GetChild(0).GetComponent<TMP_Text>().text);
        Debug.Log("Correct Answer: " + currentQuestion.GetCorrectAnswer());

        if (answerDropdown.transform.GetChild(0).GetComponent<TMP_Text>().text == currentQuestion.GetCorrectAnswer())
        {
            rightAnswer.SetActive(true);
            return true;
        }

        wrongAnswer.SetActive(true);

        return false;
    }

    /// <summary>
    /// Closes the Question UI menu
    /// </summary>
    public void CloseQuestionUI()
    {
        UIManager.Instance.SetHoveringState(false);
        rightAnswer.SetActive(false);
        wrongAnswer.SetActive(false);
        ActivateCanvas(false);
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
