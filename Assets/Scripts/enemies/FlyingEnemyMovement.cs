using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// This class handles the movement mechanics for flying enemies,
/// allowing them to ignore obstacles.
/// </summary>

public class FlyingEnemyMovement : EnemyMovement
{

    /// <summary>
    /// This function moves the enemy along the path.
    /// </summary>
    protected override void FixedUpdate()
    {
        if (target == null ) return;



        MoveTowardsTarget();

    }

    /// <summary>
    /// when an obstacle blocks the path the enemy can't move
    /// </summary>
    /// <param name="collision"> collider of the obstacle</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.CompareTag("Obstacle"))
        {
           
            Debug.Log("Flying enemy ignored obstacle.");
        }
    }

    /// <summary>
    /// when the obstacle is removed the enemy continues on the path 
    /// </summary>
    /// <param name="collision"> collider of the obstacle</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Obstacle"))
        {
         
            Debug.Log("Flying enemy exited obstacle area, no action taken.");
        }
    }
}
