using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; } //tiles grid position

    public bool IsEmpty { get; private set; }

    private Color32 fullColor = new Color32(255, 188, 188, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

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
            if(IsEmpty)
            {
                ColorTile(emptyColor);
            }
            
            if(!IsEmpty)
            {
                ColorTile(fullColor);
            }
            else if(Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
    }

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    //places a tower on the tile
    private void PlaceTower()
    {
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedTowerBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        tower.transform.SetParent(transform);

        IsEmpty = false;

        ColorTile(Color.white);

        GameManager.Instance.BuyTower();
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
