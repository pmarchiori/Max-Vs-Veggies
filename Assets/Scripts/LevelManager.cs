using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs; 

    public float TileSize //property for returning the size of a tile
    {
        get{ return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    void Start()
    {
        CreateLevel();
    }

    private void CreateLevel() //creates our level
    {
        string[] mapData = new string[]
        {
            "111", "000", "101", "001", "110"
        };

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)); //calculate the world start point (top left corner)
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x , y, worldStart);
            }
        }
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        //creates a new tile and makes a reference for that tile in the newTile variable
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);

        //uses newTile to change the position of the tile
        newTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);
    }
}
