using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///  This script is used as the base for all the different tower types
/// </summary>
public class BaseTower : MonoBehaviour
{
    [Header("References")]
   
    [SerializeField] protected LayerMask enemyMask;
   
    [SerializeField] protected GameObject upgradeUI;
    [SerializeField] protected Button upgradeButton;
    [SerializeField] protected GameObject questionUI;
    [SerializeField] protected Button rightButton;
    [SerializeField] protected Button wrongButton;

    [Header("Attribute")]
    [SerializeField] protected float targetingRange = 5f;
   
    [SerializeField] protected float projectilePerSecond = 1f;
    [SerializeField] protected int baseUpgradeCost = 100;

    protected float targetingRangeBase;
    protected float baseProjectilePerSecond;

    protected Transform target;
    protected float timeUntilFire;

    protected int level = 1;

    private string questionScene = "Question";

    protected const float RotationOffset = 90f;
    protected const float RangeExponent = 0.4f;
    protected const float ProjectilePerSecondExponent = 0.6f;
    protected const float CostExponent = 0.8f;

    protected virtual void Start()
    {
        baseProjectilePerSecond = projectilePerSecond;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
        rightButton.onClick.AddListener(() => Answer(true));
        wrongButton.onClick.AddListener(() => Answer(false));
    }

    /// <summary>
    ///  This method targets the next enemy on the path for this tower
    /// </summary>
    protected void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    /// <summary>
    ///  This method makes sure that the enemy is in range for the tower to attack
    /// </summary>
    /// <returns> True if the target is in range, false otherwise</returns>
    protected bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
 
    //The Upgrade function will be handled by a scene in the future, the code is a placeholder for the prototype
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.Instance.SetHoveringState(false);
    }

    /// <summary>
    /// Checks the pressed button and evaluates the answer
    /// </summary>
    /// <param name="isYesButton">pressed button</param>
    /// <returns>true if the selected answer was right</returns>
    public bool Answer(bool isYesButton)
    {
        if (isYesButton)
        {
            CloseQuestionUI();
            OpenUpgradeUI();
            return true;
        }
        else
        {
            Debug.Log("wrong answer");
            CloseQuestionUI();
            return false;
        }
    }

    public void OpenQuestionUI()
    {
        UIManager.Instance.SetHoveringState(true);
        SceneManager.LoadScene(questionScene, LoadSceneMode.Additive);
    }

    public void CloseQuestionUI()
    {
        SceneManager.UnloadSceneAsync(questionScene);
        UIManager.Instance.SetHoveringState(false);
    }

    /// <summary>
    ///    Upgrades a tower to an improved version.
    /// </summary>
    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.Instance.GetCurrency()) return;

        LevelManager.Instance.SpendCurrency(CalculateCost());

        level++;
        projectilePerSecond = CalculateProjectilesPerSecond();
        targetingRange = CalculateRange();

        CloseUpgradeUI();
        Debug.Log("New Pps: " + projectilePerSecond);
        Debug.Log("New TR: " + targetingRange);
        Debug.Log("New Cost: " + CalculateCost());
    }

    /// <summary>
    /// Calculates the range a tower starts firing
    /// </summary>
    /// <returns></returns>
    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, RangeExponent);
    }

    /// <summary>
    /// Calculates the number of projectiles per second 
    /// </summary>
    /// <returns></returns>
    private float CalculateProjectilesPerSecond()
    {
        return baseProjectilePerSecond * Mathf.Pow(level, ProjectilePerSecondExponent);
    }

    /// <summary>
    /// Claculates the cocts of upgrades 
    /// </summary>
    /// <returns></returns>
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, CostExponent));
    }

    
}
