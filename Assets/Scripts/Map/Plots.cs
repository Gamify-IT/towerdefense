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
        startColor = tileSprite.color;
    }

    private void OnMouseEnter()
    {
        tileSprite.color = hoverColor;
    }

    private void OnMouseExit()
    {
        tileSprite.color = startColor;
    }

    private void OnMouseDown()
    {
        if (UIManager.main.IsHoveringUI()) return;
        
        if (towerObject != null)
        {
            // ask player a question 
            tower.OpenQuestionUI();
            return;
        }
        
        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        // check if player can buy the tower 
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Ur broke ");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        towerObject = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        tower = towerObject.GetComponent<BaseTower>(); 
    }
}
