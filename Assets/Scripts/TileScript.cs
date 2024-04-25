using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; } //tiles grid position

    public bool IsEmpty { get; private set; }

    private Color32 fullColor = new Color32(255, 188, 188, 255); //color of the tile when unavailable for tower placement
    private Color32 emptyColor = new Color32(96, 255, 90, 255); //color of the tile when available for tower placement

    public SpriteRenderer SpriteRenderer { get; set; }

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
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    //sets up the tile
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
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
        //creates the tower
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedTowerBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        tower.transform.SetParent(transform); //sets the tower as transform child of the tile

        IsEmpty = false;

        ColorTile(Color.white); //sets the color back to neutral (white)

        GameManager.Instance.BuyTower();
    }

    //sets the color on the tiles
    private void ColorTile(Color newColor) 
    {
        SpriteRenderer.color = newColor;
    }
}
