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
    private bool mouseOver = false;

    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    /// <summary>
    /// Initializes audio source at the begin
    /// </summary>
    void Start()
    {
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip=clickSound;
    }

    /// <summary>
    /// This function plays the click sound
    /// </summary>
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    /// <summary>
    ///  This function handles opening the menu
    /// </summary>
    public void ToggleMenu()
    {
        PlayClickSound();
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        isMenuOpen = !isMenuOpen;
        anim.SetBool("menuOpen", isMenuOpen);
    }

    /// <summary>
    ///  This function displays the current in game currency amount
    /// </summary>
    private void OnGUI()
    {
        currencyUI.text = LevelManager.Instance.GetCurrency().ToString();

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
        mouseOver = true;
        UIManager.Instance.SetHoveringState(true);
    }

    /// <summary>
    ///  This function sets the setHoveringState function to false if the mouse is over the menu
    /// </summary>
    /// <param name="eventData"> The mouse</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        UIManager.Instance.SetHoveringState(false);

    }
}
