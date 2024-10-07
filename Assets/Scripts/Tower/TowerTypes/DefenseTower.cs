using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DefenseTower : BaseTower
{

    [SerializeField] private Animator towerAnimator;
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
            towerAnimator.Play("idle");
            target = null;
        }
        else
        {

            towerAnimator.Play("attack");

        }
    }
}
