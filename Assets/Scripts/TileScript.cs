using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), 
            transform.position.y- (GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //sets up the tile
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = gridPos; //tiles grid position
        transform.position = worldPos; //tiles world position
        transform.SetParent(parent); //set the tiles parent in the unity editor
        
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()
    {
        if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedTowerBtn != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }

    }

    //places a tower on the tile
    private void PlaceTower()
    {
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedTowerBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        tower.transform.SetParent(transform);

        Hover.Instance.Deactivate();

        GameManager.Instance.BuyTower();
    }
}
