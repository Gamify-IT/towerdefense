using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
/// <summary>
///  This class handles the ingame menu 
/// </summary>
public class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 

  [Header("References")]
  [SerializeField] TextMeshProUGUI currencyUI;
  [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    public bool mouse_over = false;

    /// <summary>
    ///  This function handles opening the menu
    /// </summary>
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    /// <summary>
    ///  This function displays the current in game currency amount
    /// </summary>

    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();

    }


    /// <summary>
    ///  This function is used in Unity to select the tower
    /// </summary>
    public void SetSelected()
    {

    }

    /// <summary>
    ///  This function sets the setHoveringState function to true if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }

    /// <summary>
    ///  This function sets the setHoveringState function to false if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false);

    }
}
