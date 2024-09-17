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
    [SerializeField] private Animator archerAnimator;


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
            archerAnimator.Play("Archer3Idle");
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
    protected virtual void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {

            foreach (Collider2D hit in hits)
            {
                EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                if (enemy != null )
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
    protected virtual void Shoot()
    {
        TurnTowardsTarget();

        GameObject projectileObj = Instantiate(currentArrowPrefab, firingPoint.position, Quaternion.identity);
        Projectile projectileScript = projectileObj.GetComponent<Projectile>();
        projectileScript.SetTarget(target);
       
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
            archerAnimator.Play("Archer3UAttack");
            currentArrowPrefab = arrowUp;
        }
        else if (angle >= -135f && angle < -45f)
        {
            archerAnimator.Play("Archer3DAttack");
            currentArrowPrefab = arrowDown;
        }
        else if (angle >= -45f && angle < 45f)
        {
            archerAnimator.Play("Archer3RAttack");
            currentArrowPrefab = arrowRight;
        }
        else
        {
            archerAnimator.Play("Archer3LAttack");
            currentArrowPrefab = arrowLeft;
        }
    }
}
