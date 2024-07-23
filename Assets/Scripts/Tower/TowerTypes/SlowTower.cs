using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SlowTower : BaseTower
{
   
    [Header("Attribute")]
    
    [SerializeField] private float freezeTime = 1f;


    private void Update()
    {
        
      
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / pps)
            {
            Debug.Log("Freeze");
                FreezeEnemies();
                timeUntilFire = 0f;
            }
        
    }

    private void FreezeEnemies()
    {

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)
            transform.position, 0f, enemyMask);

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

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }
  
}
