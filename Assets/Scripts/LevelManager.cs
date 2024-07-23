using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  This class manages the basic functions of the levels
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
    }

    /// <summary>
    ///  This function increases the currency
    /// </summary>
    /// <param name="amount"> the amount of how much the currency is increased</param>
    public void GainCurrency(int amount)
    {
        currency += amount;
    }

    /// <summary>
    ///  This function decreases the currency
    /// </summary>
    /// <param name="amount"> the amount of how much the currency is decreased</param>
    public bool SpendCurrency(int amount)
    {
        if(amount  <= currency) {
            //buy item
            currency -= amount;
                return true; }
        else
        {
            Debug.Log("you are broke :)"); //durch UI ersetzen
                return false;
        }

    }
}
