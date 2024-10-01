using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  This class handles which tower to build
/// </summary>
public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Tower[] towers;

    private int selectedTower = 0;

    /// <summary>
    /// This function manages the singleton instance, so it initializes the <c>instance</c> variable, if not set, or
    /// deletes the object otherwise
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gets the currently selected tower
    /// </summary>
    /// <returns>cuurently selected tower</returns>
    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    /// <summary>
    /// Sets which tower is currently selected
    /// </summary>
    /// <param name="_selectedTower">selected tower</param>
    public void SetSelectedTower(int selectedTower)
    {
        this.selectedTower = selectedTower;
    }

    public int GetSelectedTowerIndex()
    {
        return selectedTower;
    }
}
