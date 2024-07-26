using System;
using UnityEngine;

/// <summary>
///  class that creates the tower object
/// </summary>
[Serializable]
public class Tower
{
    private string name;
    private int costs;
    private GameObject prefab;

    public Tower(string name, int costs, GameObject prefab)
    {
        this.name = name;
        this.costs = costs;
        this.prefab = prefab;
    }

    /// <summary>
    /// Gets the name of the tower
    /// </summary>
    /// <returns>tower name</returns>
    public string GetName()
    {
        return name;
    }

    /// <summary>
    /// Gets the cost of the tower to built 
    /// </summary>
    /// <returns>building costs</returns>
    public int GetCosts()
    {
        return costs;
    }

    /// <summary>
    /// Gets the prefab for the tower
    /// </summary>
    /// <returns>tower prefab</returns>
    public GameObject GetPrefab()
    {
        return prefab;
    }

}
