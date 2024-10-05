using UnityEngine;
using System.Collections;

/// <summary>
/// This class contains the movement mechanics of the enemies.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] private HP playerHP;

    

    [SerializeField] private Animator animator;


    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

   protected Transform target;
    private int pathIndex = 0;
    private float baseSpeed;
    private bool isObstacleOnPath = false;
    private BaseTower towerHP;
    private int enemyDamage = 10;
    public bool isVisible = true;

    private void Start()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogError("LevelManager.main is not assigned!");
            return;
        }

        baseSpeed = moveSpeed;
        if (LevelManager.Instance.GetPath() != null && LevelManager.Instance.GetPath().Length > 0)
        {
            SetNextTarget();
        }
        else
        {
            Debug.LogError("Path is not set or empty in LevelManager!");
        }

       
    }

    protected virtual void Update()
    {
        if (target == null) return;

        // Determines the next part of the path
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex < LevelManager.Instance.GetPath().Length)
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
    protected virtual void FixedUpdate()
    {
        if (target == null || isObstacleOnPath ) return;

        

        MoveTowardsTarget();
    
    }

    /// <summary>
    /// Sets the next target in the path.
    /// </summary>
    private void SetNextTarget()
    {
        target = LevelManager.Instance.GetPath()[pathIndex];
    }

    /// <summary>
    /// Handles the logic when the enemy reaches the end of the path.
    /// </summary>
    private void HandleEndOfPath()
    {
        if (playerHP != null)
        {
            
            EnemyDamage enemyDamage = GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                playerHP.TakeDamage(enemyDamage.GetDamagePoints());
            }
        }

        if (EnemySpawner.GetOnEnemyDestroy() != null)
        {
            EnemySpawner.GetOnEnemyDestroy().Invoke();
        }
        Destroy(gameObject);
       
    }
    /// <summary>
    /// Damages a tower
    /// </summary>
    private void DamageTower()
    {

        if (towerHP != null)
        {
                towerHP.TakeDamage(enemyDamage);
            
        }
    }

    /// <summary>
    /// Moves the enemy towards the current target.
    /// </summary>
    protected virtual void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        UpdateAnimation(direction);
    }

    /// <summary>
    /// Changes the animation based on the Blend tree of the enemy prefab
    /// </summary>
    /// <param name="direction"> the direction the enemy is headed to next (up, down, left, right)</param>
    private void UpdateAnimation(Vector2 direction)
    {
       
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        
        
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

    /// <summary>
    /// when an obstacle blocks the path the enemy can't move
    /// </summary>
    /// <param name="collision"> collider of the obstacle</param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Obstacle"))
        {
            isObstacleOnPath = true;
            rb.velocity = Vector2.zero; // Beende die Bewegung
            towerHP = collision.GetComponent<BaseTower>();
            if (towerHP != null)
            {
                StartCoroutine(DamageTowerCoroutine());
            }
        }
    }

    /// <summary>
    /// starts coroutine for the DamageTower function
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageTowerCoroutine()
    {
        while (towerHP != null && towerHP.GetTowerHP() > 0)
        {
            DamageTower();
            yield return new WaitForSeconds(1f); // Damage every 1 second
        }
    }

    /// <summary>
    /// when the obstacle is removed the enemy continues on the path 
    /// </summary>
    /// <param name="collision"> collider of the obstacle</param>
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Obstacle"))
        {
            isObstacleOnPath = false;
        }
    }


}
