using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SniperTower : BaseTower
{[Header("References")]
    [SerializeField] private Transform archerRotationPoint;
    [SerializeField] private GameObject arrowUp;
    [SerializeField] private GameObject arrowDown;
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject arrowRight;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Animator frogAnimator;


    [Header("Attribute")]
    [SerializeField] private float rotationSpeed = 5f;

    private GameObject currentArrowPrefab;


    protected virtual void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        //TurnTowardsTarget();
        if (!CheckTargetIsInRange())
        {
            frogAnimator.Play("idle");
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / projectilePerSecond)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }


    /// <summary>
    ///  This method targets the next enemy on the path for this tower
    /// </summary>
    protected override void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                if (enemy != null)
                {
                    target = hit.transform;
                    return;
                }
            }
        }
    }

    /// <summary>
    ///  shoots a projectile targeting an enemy
    /// </summary>
    protected void Shoot()
    {
        TurnTowardsTarget();

        GameObject projectileObj = Instantiate(currentArrowPrefab, firingPoint.position, Quaternion.identity);
        Projectile projectileScript = projectileObj.GetComponent<Projectile>();
        projectileScript.SetTarget(target);

        StealthEnemyMovement stealthEnemy = target.GetComponent<StealthEnemyMovement>();
        if (stealthEnemy != null && !stealthEnemy.isVisible)
        {
          
            stealthEnemy.Reveal();
        }

    }

    /// <summary>
    /// This method reveals invisible enemies
    /// </summary>
    private void RevealEnemies()
    {
        
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {

            for (int i = 0; i < hits.Length; i++)
            {

                RaycastHit2D hit = hits[i];

                StealthEnemyMovement em = hit.transform.GetComponent<StealthEnemyMovement>();
                em.Reveal();


            }

        }
    }

    /// <summary>
    /// rotates the tower to the targeted enemy
    /// </summary>
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + RotationOffset;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        archerRotationPoint.rotation = Quaternion.RotateTowards(archerRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void TurnTowardsTarget()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        
        if (angle >= 45f && angle < 135f)
        {
            frogAnimator.Play("attackU");
            currentArrowPrefab = arrowUp;
        }
        else if (angle >= -135f && angle < -45f)
        {
            frogAnimator.Play("attackD");
            currentArrowPrefab = arrowDown;
        }
        else if (angle >= -45f && angle < 45f)
        {
            frogAnimator.Play("attackR");
            currentArrowPrefab = arrowRight;
        }
        else
        {
            frogAnimator.Play("attackL");
            currentArrowPrefab = arrowLeft;
        }
    }
}
