using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  This class makes sure you can't place a tower while answering a question
/// </summary>
public class QuestionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouse_over = false;


    /// <summary>
    ///  If the mouse hovers over the QuestionUI towers can't be build
    /// </summary>
    /// <param name="eventData"> the mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }

    /// <summary>
    ///  If the mouse is not hovering over the QuestionUI it closes again
    /// </summary>
    /// <param name="eventData"> the mouse </param>
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }
}
