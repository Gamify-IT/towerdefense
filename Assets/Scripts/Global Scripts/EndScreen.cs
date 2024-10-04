using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
///     Manages UI elements of the end screen
/// </summary>
public class EndScreen : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI elements")]
    [SerializeField] private TMP_Text rewardsText;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private Transform resultContent;
    [SerializeField] private GameObject correctEntryPrefab;
    [SerializeField] private GameObject wrongEntryPrefab;

    [Header("Game data")]
    private GameResultData result;
    private List<QuestionData> questions;
    private List<QuestionResultData> correctAnsweredQuestions;
    private List<QuestionResultData> wrongAnsweredQuestions;

    private void Start()
    {
        Time.timeScale = 0f;   
        result = GameManager.Instance.GetGameResult();
        correctAnsweredQuestions = result.GetCorrectAnsweredQuestions();
        wrongAnsweredQuestions = result.GetWrongAnsweredQuestions();
        questions = QuestionManager.Instance.GetQuestions();
        rewardsText.text = result.GetScore().ToString() + "  " + "scores" + "  " + "and" + "  " + result.GetRewards().ToString() + "  " + "coins";
    }

    /// <summary>
    ///     Displays each question toegther with the player's answer and the result, i.e. correct or wrong
    /// </summary>
    private void ShowResults()
    {
        Debug.Log("Displaying game results...");

        foreach (Transform child in resultContent)
        {
            Destroy(child.gameObject);
        }

        if (result.GetQuestionCount() == 0)
        {
            Debug.Log("No questions available");
            return;
        }

        foreach (var question in questions)
        {
            Debug.Log($"Processing question: {question.GetText()}");

            GameObject entryPrefab = null;

            var correctAnswer = correctAnsweredQuestions.Find(r => r.GetQuestionUUID() == question.GetId());
            var wrongAnswer = wrongAnsweredQuestions.Find(r => r.GetQuestionUUID() == question.GetId());

            if (correctAnswer != null)
            {
                entryPrefab = Instantiate(correctEntryPrefab, resultContent);
                Debug.Log($"{question.GetText()} answered correctly with {correctAnswer.GetAnswer()}");
            }
            else if (wrongAnswer != null)
            {
                entryPrefab = Instantiate(wrongEntryPrefab, resultContent);
                Debug.Log($"{question.GetText()} answered incorrectly with {wrongAnswer.GetAnswer()}");
            }
            else
            {
                continue;
            }

            if (entryPrefab != null)
            {
                TMP_Text questionText = entryPrefab.transform.Find("Question Text").GetComponent<TMP_Text>();
                TMP_Text answerText = entryPrefab.transform.Find("Answer Text").GetComponent<TMP_Text>();

                questionText.text = question.GetText();

                if (correctAnswer != null)
                {
                    answerText.text = correctAnswer.GetAnswer();
                }
                else if (wrongAnswer != null)
                {
                    answerText.text = wrongAnswer.GetAnswer();
                }
                else
                {
                    answerText.text = "-";
                }
            }
        }

    }

    #region button methods
    /// <summary>
    ///     Opens the game result menu to displays all questions and their answers
    /// </summary>
    public void OpenResultMenu()
    {
        ActivateResultScreen(true);
        ShowResults();
    }

    /// <summary>
    ///     (De)activates the resuts screen
    /// </summary>
    /// <param name="status">status whether the result screen shiuld be active or not</param>
    public void ActivateResultScreen(bool status)
    {
        resultScreen.SetActive(status);
        endScreen.SetActive(!status);
    }

    /// <summary>
    ///     Quits the game and the player returns to the overworld.
    /// </summary>
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.Quit();
    }

    /// <summary>
    ///     Enables player to start the endless mode and to continue playing 
    /// </summary>
    public void StartEnlessMode()
    {
        //TODO
    }
    #endregion

    /// <summary>
    ///     This function sets the setHoveringState function to true if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.SetHoveringState(true);
    }

    /// <summary>
    ///     This function sets the setHoveringState function to false if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.SetHoveringState(false);
    }
}
