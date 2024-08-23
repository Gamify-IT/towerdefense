using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int playerHealth = 50;

    /// <summary>
    /// Reduces the player's health.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to reduce.</param>
    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            HandlePlayerDeath();
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
