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
    [SerializeField] protected Transform archerRotationPoint;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firingPoint;
    

    [Header("Attribute")]
    [SerializeField] protected float rotationSpeed = 5f;
  

    protected virtual void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / pps)
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
        GameObject projectileObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
        Projectile projectileScript = projectileObj.GetComponent<Projectile>();
        projectileScript.SetTarget(target);
    }

   
    /// <summary>
    /// rotates the tower to the targeted enemy
    /// </summary>
    protected void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + RotationOffset;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        archerRotationPoint.rotation = Quaternion.RotateTowards(archerRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

  
}
