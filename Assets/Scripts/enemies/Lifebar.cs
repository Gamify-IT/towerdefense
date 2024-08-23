using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     This class manages the life bar for all enemy types
/// </summary>
public class Lifebar : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Image hitPoints;
    [SerializeField] EnemyHealth enemyHealth;
    private int maxHitpoints;
    private Camera cam;
    #endregion

    void Start()
    {
        maxHitpoints = enemyHealth.GetHitpoints();

        // add main cam as world space camera
        cam = Camera.main;
        Canvas lifebar = GetComponent<Canvas>();
        if (lifebar != null && lifebar.renderMode == RenderMode.WorldSpace)
        {
            lifebar.worldCamera = Camera.main;
        }
    }

    void Update()
    {
        hitPoints.fillAmount = (float) enemyHealth.GetHitpoints() / maxHitpoints;
        transform.rotation = cam.transform.rotation;
    }
}
