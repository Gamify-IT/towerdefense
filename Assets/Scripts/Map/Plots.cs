using UnityEngine;

/// <summary>
/// Contains all tiles of the path enemies move on.
/// </summary>
public class Plots : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer tileSprite;
    [SerializeField] private Color hoverColor;

    private GameObject towerObject;
    private BaseTower tower;
    private Color startColor;

    private void Start()
    {
        if (tileSprite == null)
        {
            Debug.LogError("Tile sprite is not assigned!");
            return;
        }

        startColor = tileSprite.color;
    }

    /// <summary>
    /// When the mouse hovers over a buildable plot, it highlights it.
    /// </summary>
    private void OnMouseEnter()
    {
        if (tileSprite != null)
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
    }

    /// <summary>
    /// This function builds a tower on the selected plot.
    /// </summary>
    private void OnMouseDown()
    {
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
    private void BuildTower()
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

        // Spend currency and instantiate the tower
        LevelManager.Instance.SpendCurrency(towerToBuild.GetCosts());
        towerObject = Instantiate(towerToBuild.GetPrefab(), transform.position, Quaternion.identity);
        tower = towerObject.GetComponent<BaseTower>();

        if (tower == null)
        {
            Debug.LogError("The instantiated tower does not have a BaseTower component!");
        }
    }
}
