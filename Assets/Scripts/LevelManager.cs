using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void GainCurrency(int amount)
    {
        currency += amount;
    }

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
