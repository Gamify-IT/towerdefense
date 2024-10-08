using UnityEngine;

public class StealthEnemyMovement : EnemyMovement
{
    [Header("References")]
    [SerializeField] private float visibilityDuration = 5f; 
    [SerializeField] private SpriteRenderer enemyRenderer;  
    [SerializeField] private Collider2D enemyCollider;

    [Header("Attributes")]  
    private float visibilityTimer = 0f;
    float transparency = 0.3f;
    float visible = 1f;

    /// <summary>
    /// enemy is invisible in the beginning
    /// </summary>
    public void Awake()
    {
        Invisible();   
    }

    protected override void Update()
    {
        base.Update();
        if (isVisible)
        {
            visibilityTimer -= Time.deltaTime;
            if (visibilityTimer <= 0f)
            {
                Invisible();
            }
        }
    }
    /// <summary>
    /// Makes the enemy visible.
    /// </summary>
    public void Reveal()
    {
        isVisible = true;
        Color color = enemyRenderer.color;
        color.a = visible;
        enemyRenderer.color = color;
        visibilityTimer = visibilityDuration;
    }

    /// <summary>
    /// Makes the enemy partially transparent (invisible).
    /// </summary>
    private void Invisible()
    {
        isVisible = false;
        Color color = enemyRenderer.color; 
        color.a = transparency;            
        enemyRenderer.color = color;      
    }

}
