using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
  
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int projectileDamage = 1;

    private Transform target;

    /// <summary>
    /// Sets the target for the projectile 
    /// </summary>
    /// <param name="target">The tower being targeted</param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

       
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * projectileSpeed;
    }

    /// <summary>
    /// Hits the tower and deals damage.
    /// </summary>
    /// <param name="other">The collider of the object hit</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the projectile hit a tower
        if (other.gameObject.CompareTag("Tower"))
        {
            BaseTower tower = other.gameObject.GetComponent<BaseTower>();
            if (tower != null)
            {
                tower.TakeDamage(projectileDamage); // Deal damage to the tower
            }

            Destroy(gameObject); // Destroy the projectile after impact
        }
    }
}



