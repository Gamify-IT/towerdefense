using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  This class makes sure you cannot place a tower while in the UpgradeUI.
/// </summary>
public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouseOver = false;

    /// <summary>
    ///  If the mouse hovers over the UpgradeUI towers can't be build
    /// </summary>
    /// <param name="eventData"> the mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        UIManager.Instance.SetHoveringState(true);
    }

    /// <summary>
    ///  If the mouse is not hovering over the UpgradeUI it closes again
    /// </summary>
    /// <param name="eventData"> the mouse </param>
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        UIManager.Instance.SetHoveringState(false);
        gameObject.SetActive(false);
    }
}
