using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int damagePoints = 2;
    

    /// <summary>
    /// Returns the damage points of the enemy.
    /// </summary>
    /// <returns>Damage points of the enemy.</returns>
    public int GetDamagePoints()
    {
        return damagePoints;
    }


}
