using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [Header("Attributes")]
    public RectTransform healthbar;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private float originalWidth;

    private void Start()
    {
        currentHealth = maxHealth;
        originalWidth = healthbar.sizeDelta.x;
        UpdateHealthBar();
    }
    
    /// <summary>
    /// Reduces the player's health.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to reduce.</param>
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            HandlePlayerDeath();
        }
    }

    /// <summary>
    /// Readjust the healthbar width after damage has been taken in.
    /// </summary>
    private void UpdateHealthBar()
    {
        float newWidth = originalWidth * (currentHealth / maxHealth);
        healthbar.sizeDelta = new Vector2(newWidth, healthbar.sizeDelta.y);
    }

    /// <summary>
    /// Game ends when the player reaches zero HP
    /// </summary>
    private void HandlePlayerDeath()
    {
        Debug.Log("Player has died! Loading game over scene.");
        SceneManager.LoadScene("GameOver");
    }
}
