using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// class for the tower that freezes enemies periodically
/// </summary>
public class SlowTower : BaseTower
{
    [Header("Attribute")]
    [SerializeField] private float freezeTime = 1f;

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / projectilePerSecond)
        {
        Debug.Log("Freeze");
            FreezeEnemies();
            timeUntilFire = 0f;
        }  
    }

    /// <summary>
    /// This method slows enemies down
    /// </summary>
    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.position, 0f, enemyMask);

        if(hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    /// <summary>
    /// after the freeze wears off enemies continue walking in normal speed again
    /// </summary>
    /// <param name="em"> the enemy movement</param>
    /// <returns>enumerator for resetting the enemy speed</returns>
    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }
  
}
