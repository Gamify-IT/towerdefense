using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    }
    

    public void SetQuestions(List<QuestionData> questions)
    {
        this.questions = questions;
        this.questions.ForEach(question => Debug.Log(question.GetText()));
    }


    /// <summary>
    /// Loads the current question to the text and dropdown menu of the question scene
    /// </summary>
    public void LoadQuestion()
    {
        Debug.Log("Loading Question...");
#if UNITY_EDITOR
        questionText.text = "1+1";
        answerDropdown.GetComponent<TMP_Dropdown>().options.Clear();
        answerDropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData("2"));
#else
        currentQuestion = questions[questionCounter];
        questionText.text = currentQuestion.GetText();
        answerDropdown.GetComponent<TMP_Dropdown>().options.Clear();
        answerDropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(currentQuestion.GetCorrectAnswer()));
        foreach (var question in currentQuestion.GetWrongAnswers()) { answerDropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(question)); }
        questionCounter++;
#endif
    }


    public void SetActive(bool active)
    {
        if (!active)
        {
            canvas.SetActive(true);
        }
        else 
        {
            canvas.SetActive(false);
        }
    }


    /// <summary>
    /// Checks if the given answer is correct
    /// </summary>
    public bool CheckAnswer()
    {
#if !UNITY_EDITOR
        if (answerDropdown.GetComponentInChildren<Label>().text == currentQuestion.GetCorrectAnswer())
        {
            return true;
        }
        return false;
#else
        return true;
#endif
    }

    private void Quit()
    {
        SceneManager.UnloadSceneAsync("Question");
        UIManager.Instance.SetHoveringState(false);
    }
}
