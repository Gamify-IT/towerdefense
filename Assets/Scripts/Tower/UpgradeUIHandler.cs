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

    private AudioSource audioSource;
    [SerializeField] private AudioClip updateTowerSound;

    /// <summary>
    /// Initializes audio source at the begin
    /// </summary>
    void Start()
    {
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        updateTowerSound = Resources.Load<AudioClip>("Music/update_tower");
        audioSource.clip=updateTowerSound;
    }

    /// <summary>
    /// This function plays the tower update sound
    /// </summary>
    public void PlayUpdateTowerSound()
    {
        if (updateTowerSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(updateTowerSound);
        }
    }

    /// <summary>
    ///  If the mouse hovers over the UpgradeUI towers can't be build
    /// </summary>
    /// <param name="eventData"> the mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayUpdateTowerSound();
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
