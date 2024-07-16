using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Build;

public class BaseTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform archerRotationPoint;
    [SerializeField] private LayerMask enemyMask; //nur enemies attackieren
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject questionUI;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button wrongButton;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pps = 1f; //projectile per second
    [SerializeField] private int baseUpgradeCost = 100;

    private float targetingRangeBase;
    private float ppsBase; 

    private Transform target;
    private float timeUntilFire;

    private int level = 1;

    private void Start()
    {
        ppsBase = pps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
        rightButton.onClick.AddListener(() => Answer(true));
        wrongButton.onClick.AddListener(() => Answer(false));
    }
    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / pps) {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }
    private void Shoot()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
        Projectile projectileScript  = projectileObj.GetComponent<Projectile>();
        projectileScript.SetTarget(target);
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)
            transform.position, 0f, enemyMask);

        if(hits.Length > 0)
        {
            target = hits[0].transform; //rotate player towards target
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - 
            transform.position.x) * Mathf.Rad2Deg +90f; //wenn falschrum anvisiert zu -90 wechseln

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        archerRotationPoint.rotation = Quaternion.RotateTowards(archerRotationPoint.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }

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
        Debug.Log("New Pps: "+ pps);
        Debug.Log("New TR: " + targetingRange);
        Debug.Log("New Cost: " + CalculateCost());
    }

    

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    private float CalculatePPS()
    {
        return ppsBase* Mathf.Pow(level, 0.6f);
    }
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
