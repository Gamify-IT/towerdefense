using TMPro;
using UnityEngine;
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
    [SerializeField] protected TMP_Text levelLabel;
    [SerializeField] protected TMP_Text upgradePriceLabel;
    [SerializeField] protected Canvas lifeBar;
    [SerializeField] protected Image lifeBarFill;

    [Header("Attributes")]
    [SerializeField] protected float targetingRange = 5f; 
    [SerializeField] protected float projectilePerSecond = 1f;
    [SerializeField] protected int baseUpgradeCost = 100;
    [SerializeField] protected int towerHP = 10;

    [Header("Audio Elements")]
    private AudioClip updateTowerSound;
    private AudioSource mainAudioSource;

    protected float targetingRangeBase;
    protected float baseProjectilePerSecond;

    protected int maxTowerHP;

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
        maxTowerHP = towerHP;

        if (lifeBar != null && lifeBar.renderMode == RenderMode.WorldSpace)
        {
            lifeBar.worldCamera = Camera.main;
        }

        upgradeButton.onClick.AddListener(Upgrade);
        InitAudio();
    }

    protected virtual void Update()
    {
        //UpdateLifeBar();
    }

    /// <summary>
    /// Initializes all audio components
    /// </summary>
    private void InitAudio()
    {
        if (mainAudioSource == null)
        {
            mainAudioSource = gameObject.AddComponent<AudioSource>();
        }
        updateTowerSound = Resources.Load<AudioClip>("Music/update_tower");
        mainAudioSource.clip = updateTowerSound;
    }

    /// <summary>
    ///  This method targets the next enemy on the path for this tower
    /// </summary>
    protected virtual void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
           foreach (Collider2D hit in hits)
            {
                EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                if (enemy != null && enemy.isVisible)
                {
                    target = hit.transform;
                    return; 
                }
            }
        }
    }
    /// <summary>
    /// Reduces the tower's health.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to reduce.</param>
    public void TakeDamage(int damageAmount)
    {
        towerHP -= damageAmount;
        Debug.Log("Tower Health: " + towerHP);
        if (towerHP <= 0)
        {
            Debug.Log("Tower Destroyed");
            Destroy(gameObject);
        }

        UpdateLifeBar();
    }

    /// <summary>
    /// returns the HP of the tower
    /// </summary>
    /// <returns></returns>
    public int GetTowerHP()
    {
        return towerHP;
    }

    /// <summary>
    ///  This method makes sure that the enemy is in range for the tower to attack
    /// </summary>
    /// <returns> True if the target is in range, false otherwise</returns>
    protected bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
 
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
        upgradePriceLabel.text = "Upgrade: " + CalculateCost().ToString();
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
    }

    /// <summary>
    /// Checks the selected answer and evaluates it. If the answer is right, the tower will be upgraded. 
    /// If not, the player looses credits.
    /// </summary>
    /// <param name="answer">the players answer</param>
    public void Answer(bool answer)
    {
        if (answer)
        {
            Debug.Log("correct answer");

            LevelManager.Instance.SpendCurrency(CalculateCost());

            level++;
            projectilePerSecond = CalculateProjectilesPerSecond();
            targetingRange = CalculateRange();

            levelLabel.text = level.ToString();

            Debug.Log("New Pps: " + projectilePerSecond);
            Debug.Log("New TR: " + targetingRange);
            Debug.Log("New Cost: " + CalculateCost());

        }
        else
        {
            Debug.Log("wrong answer");

            // TODO: adjust punishment
            LevelManager.Instance.SpendCurrency(CalculateCost());
        }

        CloseUpgradeUI();
        PlayUpdateTowerSound();
    }

    /// <summary>
    ///     Updates the lifebar above towers by displaying the remaining hit points
    /// </summary>
    private void UpdateLifeBar()
    {
        lifeBarFill.fillAmount = (float)towerHP / maxTowerHP;
        transform.rotation = Camera.main.transform.rotation;
    }

    public void OpenQuestionUI()
    {
        UIManager.Instance.SetHoveringState(true);
        QuestionManager.Instance.ActivateCanvas(true);
    }

    /// <summary>
    ///    Upgrades a tower to an improved version.
    /// </summary>
    public void Upgrade()
    {
        QuestionManager.Instance.SetUpgradedTower(this);

        if (CalculateCost() > LevelManager.Instance.GetCurrency())
        {
            StartCoroutine(PauseButton.Instance.ShowFeedbackWindow("Not enough Credits!"));
            return;
        }

        Debug.Log("Opening Question Menu..."); 
        bool successfull = QuestionManager.Instance.LoadQuestion();

        if (successfull)
        {
            QuestionManager.Instance.OpenQuestionUI();
        }
    }

    /// <summary>
    /// (De)activates the submit button of the question only to the current tower that is upgraded 
    /// </summary>
    /// <param name="status">whether the submit button is active or not for this tower</param>
    public void AssignSubmitButton(bool status)
    {
        if (status)
        {
            QuestionManager.Instance.submitButton.onClick.AddListener(() => Answer(QuestionManager.Instance.CheckAnswer()));
        }      
        else
        {
            QuestionManager.Instance.submitButton.onClick.RemoveAllListeners();
        }
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
    /// Calculates the costs of upgrades 
    /// </summary>
    /// <returns></returns>
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, CostExponent));
    }


    /// <summary>
    /// This function plays the tower update sound
    /// </summary>
    public void PlayUpdateTowerSound()
    {
        if (updateTowerSound != null && mainAudioSource != null)
        {
            mainAudioSource.PlayOneShot(updateTowerSound);
        }
    }

}
