using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth;
    [SerializeField] private int damagePoints;

    private bool isDestroyed = false;

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
            EnemySpawner.GetOnEnemyDestroy().Invoke();
            LevelManager.Instance.GainCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public int GetDamagePoints()
    {
        damagePoints = hitPoints;
        return damagePoints;

    }


}
