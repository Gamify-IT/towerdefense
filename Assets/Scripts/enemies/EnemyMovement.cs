using UnityEngine;

/// <summary>
/// This class contains the movement mechanics of the enemies.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private float baseSpeed;

    private void Start()
    {
        if (LevelManager.main == null)
        {
            Debug.LogError("LevelManager.main is not assigned!");
            return;
        }

        baseSpeed = moveSpeed;
        if (LevelManager.main.path != null && LevelManager.main.path.Length > 0)
        {
            SetNextTarget();
        }
        else
        {
            Debug.LogError("Path is not set or empty in LevelManager!");
        }
    }

    private void Update()
    {
        if (target == null) return;

        // Determines the next part of the path
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex < LevelManager.main.path.Length)
            {
                SetNextTarget();
            }
            else
            {
                HandleEndOfPath();
            }
        }
    }

    /// <summary>
    /// This function moves the enemy along the path.
    /// </summary>
    private void FixedUpdate()
    {
        if (target == null) return;

        MoveTowardsTarget();
    }

    /// <summary>
    /// Sets the next target in the path.
    /// </summary>
    private void SetNextTarget()
    {
        target = LevelManager.main.path[pathIndex];
    }

    /// <summary>
    /// Handles the logic when the enemy reaches the end of the path.
    /// </summary>
    private void HandleEndOfPath()
    {
        if (EnemySpawner.onEnemyDestroy != null)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Moves the enemy towards the current target.
    /// </summary>
    private void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    /// <summary>
    /// Updates the enemies' speed.
    /// </summary>
    /// <param name="newSpeed">New speed value for the enemies.</param>
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    /// <summary>
    /// Resets the movement speed of the enemies to the base speed.
    /// </summary>
    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }
}
