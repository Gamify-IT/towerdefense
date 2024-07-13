using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform archerRotationPoint;
    [SerializeField] private LayerMask enemyMask; //nur enemies attackieren
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pps = 1f; //projectile per second

    private Transform target;
    private float timeUntilFire;

    private void Update()
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

            if (timeUntilFire >= 1f / pps) {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }
    private void Shoot()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
        Projectile projectileScript  = projectileObj.GetComponent<Projectile>();
        projectileScript.SetTarget(target);
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)
            transform.position, 0f, enemyMask);

        if(hits.Length > 0)
        {
            target = hits[0].transform; //rotate player towards target
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - 
            transform.position.x) * Mathf.Rad2Deg +90f; //wenn falschrum anvisiert zu -90 wechseln

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        archerRotationPoint.rotation = Quaternion.RotateTowards(archerRotationPoint.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
