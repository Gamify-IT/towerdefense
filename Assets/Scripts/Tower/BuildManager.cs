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

    [SerializeField] private GameObject defenseTowerPrefabUp;
    [SerializeField] private GameObject defenseTowerPrefabDown;
    [SerializeField] private GameObject defenseTowerPrefabLeft;
    [SerializeField] private GameObject defenseTowerPrefabRight;

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

    /// <summary>
    /// Selects the correct prefab for the defense tower based on the relative position to the path
    /// </summary>
    /// <param name="relativePosition">The given relative position on enemy path</param>
    /// <returns>The correct prefab for the defense tower</returns>
    public GameObject SelectPrefabBasedOnPath(Vector2 relativePosition)
    {
        // Get the angle in degrees from the relative position
        float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;

        Debug.Log($"Relative Position: {relativePosition}, Angle: {angle}");

        if (angle >= -45 && angle < 45)
        {
            // Enemy path is to the right
            return defenseTowerPrefabRight;
        }
        else if (angle >= 45 && angle < 135)
        {
            // Enemy path is above
            return defenseTowerPrefabUp;
        }
        else if (angle >= 135 || angle < -135)
        {
            // Enemy path is to the left
            return defenseTowerPrefabLeft;
        }
        else
        {
            // Enemy path is below
            return defenseTowerPrefabDown;
        }
    }
        
    public int GetSelectedTowerIndex()
    {
        return selectedTower;
    }
}
