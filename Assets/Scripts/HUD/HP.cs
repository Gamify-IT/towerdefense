using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public static HP Instance {get; private set;}

        [Header("Attributes")]
    [SerializeField] private int maxHealth = 50;

    [Header("References")]
    [SerializeField] private Image healthBar;

    private int playerHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerHealth = maxHealth;  
        UpdateHealthBar();
        Debug.Log("HP Script initialized with player health: " + playerHealth);
    }
    
    /// <summary>
    /// Reduces the player's health.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to reduce.</param>
    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        Debug.Log("Player Health: " + playerHealth);

        UpdateHealthBar();

        if (playerHealth <= 0)
        {
            HandlePlayerDeath();
        }
    }

    /// <summary>
    /// Updates the visual health bar to reflect the current health.
    /// </summary>
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)playerHealth / maxHealth;  // Calculate fill amount (between 0 and 1)
        }
        else
        {
            Debug.LogWarning("Health bar UI Image is not assigned!");
        }
    }

    /// <summary>
    /// Game ends when the player reaches zero HP
    /// </summary>
    private void HandlePlayerDeath()
    {
        // Game ends here
        Debug.Log("Player has died!");
    }
}
