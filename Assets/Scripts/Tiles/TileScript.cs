using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; } //tiles grid position

    private SpriteRenderer spriteRenderer;

    private Toys toy;

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
            if(toy != null)
            {
                GameManager.Instance.SelectTower(toy);
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
        // Create the tower      
        GameObject toyObj = (GameObject)Instantiate(GameManager.Instance.ClickedTowerBtn.TowerPrefab, transform.position, Quaternion.identity);
        toyObj.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        toyObj.transform.SetParent(transform); // Sets the tower as transform child of the tile

        this.toy = toyObj.GetComponentInChildren<Toys>(); // Get the Toy component from the instantiated GameObject

        if (this.toy != null)
        {
            this.toy.Price = GameManager.Instance.ClickedTowerBtn.Price; // Set the Price on the Turret component
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
