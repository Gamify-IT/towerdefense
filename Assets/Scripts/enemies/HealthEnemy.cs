using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth;

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
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.GainCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
