using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; } //tiles grid position

    private SpriteRenderer spriteRenderer;

    private Turret turret;

    private Color32 fullColor = new Color32(255, 188, 188, 255); //color of the tile when unavailable for tower placement
    private Color32 emptyColor = new Color32(96, 255, 90, 255); //color of the tile when available for tower placement

    public bool IsEmpty { get;  set; }
    public bool Walkable { get; set; }

    public bool Debugging { get; set; }

    public Vector2 WorldPosition //tiles center wold position
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), 
            transform.position.y- (GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //sets up the tile
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        Walkable = true;
        IsEmpty = true;

        this.GridPosition = gridPos; //tiles grid position
        transform.position = worldPos; //tiles world position
        transform.SetParent(parent); //set the tiles parent in the unity editor
        
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()
    {
        if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedTowerBtn != null)
        {
            if(IsEmpty && !Debugging)
            {
                ColorTile(emptyColor);
            }
            
            if(!IsEmpty && !Debugging)
            {
                ColorTile(fullColor);
            }
            else if(Input.GetMouseButtonDown(0)) //places a tower if the tile is available (empty)
            {
                PlaceTower();
            }
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedTowerBtn == null && Input.GetMouseButtonDown(0))
        {
            if(turret != null)
            {
                GameManager.Instance.SelectTower(turret);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()
    {
        if(!Debugging)
        {
            ColorTile(Color.white);
        }  
    }

    //places a tower on the tile
    private void PlaceTower()
    {
        // //creates the tower
        // GameObject turret = (GameObject)Instantiate(GameManager.Instance.ClickedTowerBtn.TowerPrefab, transform.position, Quaternion.identity);
        // turret.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        // turret.transform.SetParent(transform); //sets the tower as transform child of the tile

        // this.turret = turret.transform.GetChild(0).GetComponent<Turret>();

        // IsEmpty = false;

        // ColorTile(Color.white); //sets the color back to neutral (white)

        // turret.Price = GameManager.Instance.ClickedTowerBtn.Price;

        // GameManager.Instance.BuyTower();

        // Walkable = false;




        // Create the tower      
        GameObject turretObj = (GameObject)Instantiate(GameManager.Instance.ClickedTowerBtn.TowerPrefab, transform.position, Quaternion.identity);
        turretObj.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        turretObj.transform.SetParent(transform); // Sets the tower as transform child of the tile

        this.turret = turretObj.GetComponentInChildren<Turret>(); // Get the Turret component from the instantiated GameObject

        if (this.turret != null)
        {
            this.turret.Price = GameManager.Instance.ClickedTowerBtn.Price; // Set the Price on the Turret component
        }
        else
        {
            Debug.LogError("Turret component not found on the instantiated GameObject.");
        }

        IsEmpty = false;

        ColorTile(Color.white); // Sets the color back to neutral (white)

        GameManager.Instance.BuyTower();

        Walkable = false;
    }

    //sets the color on the tiles
    private void ColorTile(Color newColor) 
    {
        spriteRenderer.color = newColor;
    }
}
