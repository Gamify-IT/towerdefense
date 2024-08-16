using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Build;

/// <summary>
/// Class for the simple tower that shoots enemies and does damage
/// </summary>
public class SimpleTower : BaseTower
{
    [Header("References")]
    [SerializeField] private Transform archerRotationPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Animator archerAnimator;


    [Header("Attribute")]
    [SerializeField] private float rotationSpeed = 5f;

     
    
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
    ///  shoots a projectile targeting an enemy
    /// </summary>
    protected virtual void Shoot()
    {
        TurnTowardsTarget();

        GameObject projectileObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
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
        }
        else if (angle >= -135f && angle < -45f)
        {
            archerAnimator.Play("Archer3DAttack");
        }
        else if (angle >= -45f && angle < 45f)
        {
            archerAnimator.Play("Archer3RAttack");
        }
        else
        {
            archerAnimator.Play("Archer3LAttack");
        }
    }

}
