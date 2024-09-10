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
}
