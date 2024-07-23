using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

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
   
    [SerializeField] protected float pps = 1f;
    [SerializeField] protected int baseUpgradeCost = 100;

    protected float targetingRangeBase;
    protected float ppsBase;

    protected Transform target;
    protected float timeUntilFire;

    protected int level = 1;

    protected const float RotationOffset = 90f;
    protected const float RangeExponent = 0.4f;
    protected const float PPSExponent = 0.6f;
    protected const float CostExponent = 0.8f;

    protected virtual void Start()
    {
        ppsBase = pps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
        rightButton.onClick.AddListener(() => Answer(true));
        wrongButton.onClick.AddListener(() => Answer(false));
    }

   

  

    protected void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    protected bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

   
    //The Upgrade function will be handled by a scene in the future
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

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
        questionUI.SetActive(true);
    }

    public void CloseQuestionUI()
    {
        questionUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;
        pps = CalculatePPS();
        targetingRange = CalculateRange();

        CloseUpgradeUI();
        Debug.Log("New Pps: " + pps);
        Debug.Log("New TR: " + targetingRange);
        Debug.Log("New Cost: " + CalculateCost());
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, RangeExponent);
    }

    private float CalculatePPS()
    {
        return ppsBase * Mathf.Pow(level, PPSExponent);
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, CostExponent));
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
