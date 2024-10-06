using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
///     Manages the health of enemies including taking damage and deleting killed ennemies
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    #region attributes
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth;
    [SerializeField] private Image lifebarFill;

    private AudioSource audioSource;
    [SerializeField] private AudioClip enemyKillSound;

    private Camera cam;
    private int maxHitPoints;
    private bool isDestroyed = false;
    #endregion

    void Start()
    {
        maxHitPoints = hitPoints;

        // set main camera as world space camera
        cam = Camera.main;
        Canvas lifeBar = transform.GetChild(0).GetComponent<Canvas>();

        if (lifeBar != null && lifeBar.renderMode == RenderMode.WorldSpace)
        {
            lifeBar.worldCamera = Camera.main;
        }

        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        enemyKillSound = Resources.Load<AudioClip>("Music/kill_enemy");
        audioSource.clip=enemyKillSound;
    }

    void Update()
    {
        UpdateLifeBar();
    }

    /// <summary>
    /// Deals damage to the enemy by decreasing his hit points.
    /// </summary>
    /// <param name="damageAmount">amount of damage the enemy should take</param>
    public void TakeDamage(int damageAmount)
    {
        hitPoints -= damageAmount;

        // check if enemy is dead, i.e., should be destroyed
        if (hitPoints <= 0 && !isDestroyed) 
        {
            DestroyEnemy();
        }
    }

    /// <summary>
    /// Plays sound and after that destroy enemy that was killed
    /// </summary>
    private void DestroyEnemy()
    {
        PlayEnemyKillSound();
        //yield return new WaitForSeconds(0.45f);
        EnemySpawner.GetOnEnemyDestroy().Invoke();
        LevelManager.Instance.GainCurrency(currencyWorth);
        isDestroyed = true;
        Destroy(gameObject);
    }

    /// <summary>
    ///     Updates the lifebar above enemies by displaying the remaining hit points
    /// </summary>
    private void UpdateLifeBar()
    {
        lifebarFill.fillAmount = (float)hitPoints / maxHitPoints;
        transform.rotation = cam.transform.rotation;
    }

    /// <summary>
    /// This function plays the kill enemy sound
    /// </summary>
    private void PlayEnemyKillSound()
    {
        if (enemyKillSound != null)
        {
            audioSource.PlayOneShot(enemyKillSound);
        }
    }

}
