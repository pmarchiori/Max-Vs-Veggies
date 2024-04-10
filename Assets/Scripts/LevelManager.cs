using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;

    [SerializeField] private CameraMovement cameraMovement;

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
        string[] mapData = ReadLevelText();

        int mapX = mapData[0].ToCharArray().Length; //calculates the x map size
        int mapY = mapData.Length; //calculates the y map size

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)); //calculate the world start point (top left corner)
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                maxTile = PlaceTile(newTiles[x].ToString(), x , y, worldStart); //places the tiles
            }
        }
        //sets the camera limits to the max tile position
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y -TileSize));
    }

    private Vector3 PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        //parses tileType into an int to use if as an indexer when creating a new tile
        int tileIndex = int.Parse(tileType);

        //creates a new tile and makes a reference for that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //uses newTile to change the position of the tile
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0));

        return newTile.transform.position;
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }
}
