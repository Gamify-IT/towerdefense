using UnityEditor;
using UnityEngine;

/// <summary>
/// Contains all tiles of the path enemies move on.
/// </summary>
public class Plots : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer tileSprite;
    [SerializeField] private Color hoverColor;

    [Header("Path Validation")]
    [SerializeField] private float pathTolerance = 0.5f; // Distance to consider plot as part of the path
    [SerializeField] private Transform[] pathPoints; // All path points defining the path

    private bool isBuildable = true; // New: bool for determining, if this plot is buildable
    protected GameObject towerObject;
    protected BaseTower tower;
    private Color startColor;

    // Static path points shared across all Plots instances
    public static Transform[] PathPoints { get; set; }

    private void Start()
    {
        if (tileSprite == null)
        {
            Debug.LogError("Tile sprite is not assigned!");
            return;
        }

        startColor = tileSprite.color;

        // New: Validate if this plot is on the path
        ValidateBuildable();
    }

    /// <summary>
    /// Validates whether this plot is buildable based on its position relative to the path.
    /// </summary>
    private void ValidateBuildable()
    {
        Vector3 plotPosition = transform.position;

        if (PathPoints == null || PathPoints.Length < 2)
        {
            Debug.LogError("Path points are not properly assigned!");
            return;
        }

        for (int i = 0; i < pathPoints.Length - 1; i++)
        {
            Vector3 pointA = pathPoints[i].position;
            Vector3 pointB = pathPoints[i + 1].position;

            if (IsPointNearLineSegment(plotPosition, pointA, pointB, pathTolerance))
            {
                isBuildable = false; // Plot is too close to the path
                Debug.Log($"Plot at {plotPosition} is on the path and is not buildable.");
                return;
            }
        }
    }

    /// <summary>
    /// Checks if a point is near a line segment defined by two points.
    /// </summary>
    /// <param name="point">The position of the plot.</param>
    /// <param name="lineStart">Start of the line segment.</param>
    /// <param name="lineEnd">End of the line segment.</param>
    /// <param name="tolerance">Maximum distance to consider the point near the line.</param>
    /// <returns>True if the point is near the line segment; otherwise, false.</returns>
    private bool IsPointNearLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd, float tolerance)
    {
        // Calculate the closest point on the line segment to the given point
        Vector3 lineDirection = lineEnd - lineStart;
        float lineLengthSquared = lineDirection.sqrMagnitude;

        if (lineLengthSquared == 0f) return Vector3.Distance(point, lineStart) < tolerance; // Handle degenerate case (lineStart == lineEnd)

        float t = Mathf.Clamp01(Vector3.Dot(point - lineStart, lineDirection) / lineLengthSquared);
        Vector3 closestPoint = lineStart + t * lineDirection;

        // Check the distance from the plot to the closest point
        return Vector3.Distance(point, closestPoint) < tolerance;
    }
    

    /// <summary>
    /// When the mouse hovers over a buildable plot, it highlights it.
    /// </summary>
    private void OnMouseEnter()
    {
        if (!isBuildable) return; //Prevent hover interaction if not buildable
        if (tileSprite != null && !UIManager.Instance.IsHoveringUI())
        {
            tileSprite.color = hoverColor;
        }
    }

    /// <summary>
    /// When the mouse stops hovering over a buildable plot, the highlight goes away.
    /// </summary>
    private void OnMouseExit()
    {
        if (tileSprite != null)
        {
            tileSprite.color = startColor;
        }
        if (tower != null)
        {
            tower.CloseUpgradeUI();
        }
    }

    /// <summary>
    /// This function builds a tower on the selected plot.
    /// </summary>
    private void OnMouseDown()
    {
        if (!isBuildable) return; // Prevent building if not buildable.
        if (GameManager.Instance.IsFinished()) return;

        if (UIManager.Instance == null || BuildManager.Instance == null || LevelManager.Instance == null)
        {
            Debug.LogError("Managers are not properly assigned!");
            return;
        }

        if (UIManager.Instance.IsHoveringUI()) return;

        if (towerObject != null)
        {
            tower.OpenUpgradeUI();
            return;
        }

        BuildTower();
    }

    /// <summary>
    /// Handles the logic for building a tower.
    /// </summary>
    protected virtual void BuildTower()
    {
        Tower towerToBuild = BuildManager.Instance.GetSelectedTower();

        // check if a tower is currently selected
        if (towerToBuild == null)
        {
            StartCoroutine(PauseButton.Instance.ShowFeedbackWindow("No Tower selected!"));
            return;
        }

        // Check if the player can afford the tower
        if (towerToBuild.GetCosts() > LevelManager.Instance.GetCurrency())
        {
            StartCoroutine(PauseButton.Instance.ShowFeedbackWindow("Not enough Credits!"));
            return;
        }

        // Check if the selected tower is a defense tower (since it needs special instantiation)
        if (towerToBuild.GetName().Equals("Defense"))
        {
            BuildDefenseTower();
        }
        else
        {
            // For the other towers, use the normal instantiation logic
            towerObject = Instantiate(towerToBuild.GetPrefab(), new Vector3(transform.position.x, transform.position.y, 2), Quaternion.identity);
        }

        if (towerObject != null)
        {
            tower = towerObject.GetComponent<BaseTower>();
        }

        if (tower == null)
        {
            Debug.Log("The instantiated tower does not have a BaseTower component!");
        }
        else
        {
            LevelManager.Instance.SpendCurrency(towerToBuild.GetCosts());
        }
    }

    /// <summary>
    /// Instantiate the defense tower at the selected position with an offset and 
    /// the correct prefab for that position.
    /// </summary>
    protected virtual void BuildDefenseTower()
    {
        // Get relative position to the enemy path and select the correct prefab
        Vector2 relativePosition = GetRelativePositionToPath();

        // Ensure the tower is close enough to the path
        if (relativePosition.magnitude > 1.5f)
        {
            StartCoroutine(PauseButton.Instance.ShowFeedbackWindow("Invalid placement for defense tower. It must be next to the enemy path."));
            return;
        }

        GameObject selectedPrefab = BuildManager.Instance.SelectPrefabBasedOnPath(relativePosition);

        // Calculate an offset to ensure part of the prefab (the guard) overlaps the path
        float overlapOffset = 1.0f;
        Vector3 finalPosition = new Vector3(transform.position.x, transform.position.y, 2) + (Vector3)(relativePosition.normalized * overlapOffset);

        // Instantiate the defense tower with the selected prefab
        towerObject = Instantiate(selectedPrefab, finalPosition, Quaternion.identity);
    }

    /// <summary>
    /// Calculates the closest point on the path by interpreting the path as segments
    /// from one path point to the other.
    /// </summary>
    /// <returns>Relative position to the closest path point</returns>
    private Vector2 GetRelativePositionToPath()
    {
        Transform[] enemyPathPoints = LevelManager.Instance.GetPath();
        Vector2 towerPosition = transform.position;

        Vector2 closestPoint = Vector2.zero;
        float shortestDistance = Mathf.Infinity;

        // Iterate over each path segment
        for (int i = 0; i < enemyPathPoints.Length - 1; i++)
        {
            Vector2 segmentStart = enemyPathPoints[i].position;
            Vector2 segmentEnd = enemyPathPoints[i + 1].position;

            // Get the closest point on the line segment
            Vector2 closestPointOnSegment = ClosestPointOnLineSegment(segmentStart, segmentEnd, towerPosition);

            // Calculate the distance to the closest point on the segment
            float distance = Vector2.Distance(towerPosition, closestPointOnSegment);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPoint = closestPointOnSegment;
            }
        }

        return closestPoint - towerPosition;
    }

    /// <summary>
    /// Calculates the closest point on the enemy path (line segment between enemy path points) 
    /// to the selected tower position.
    /// </summary>
    /// <param name="pointA"> Starting path point on enemy path segment</param>
    /// <param name="pointB"> Ending path point on enemy path segment</param>
    /// <param name="position">The tower position for which we want the closest point on enemy path</param>
    /// <returns></returns>
    private Vector2 ClosestPointOnLineSegment(Vector2 pointA, Vector2 pointB, Vector2 position)
    {
        Vector2 AP = position - pointA;
        Vector2 AB = pointB - pointA;
        float magnitudeAB = AB.sqrMagnitude;
        float ABAPproduct = Vector2.Dot(AP, AB);
        float distance = ABAPproduct / magnitudeAB;

        if (distance < 0)
        {
            return pointA;
        }
        else if (distance > 1)
        {
            return pointB;
        }
        else
        {
            // Somewhere in between point A and B
            return pointA + AB * distance;
        }
    }

}
