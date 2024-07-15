using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int projectileDamage = 1;

    private Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (!target) return;
        //normalized = nur zwischen 0 und 1
        Vector2 direction = (target.position - transform.position).normalized; 
        rb.velocity = direction * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
