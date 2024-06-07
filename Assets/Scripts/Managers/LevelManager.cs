using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class LevelManager : Singleton<LevelManager>
{
    [Header("References")]
    //array of tile prefabs, used to create tiles in game
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private Transform map; 
    [SerializeField] private GameObject pointAPrefab;
    [SerializeField] private GameObject pointBPrefab;

    //spawn / despawn points
    private Point pointASpawn; 
    private Point pointBSpawn;

    public Spawn PointA { get; set; }

    private Point mapSize;

    //private Stack<Node> finalPath;

    public Dictionary<Point, TileScript> Tiles { get; set; } //dictionary that contains all tiles

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
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapX = mapData[0].ToCharArray().Length; //calculates the x map size
        int mapY = mapData.Length; //calculates the y map size

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)); //calculate the world start point (top left corner)
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x , y, worldStart); //places the tiles
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        //sets the camera limits to the max tile position
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y -TileSize));

        //SpawnPoints();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        //parses tileType into an int to use it as an indexer when creating a new tile
        int tileIndex = int.Parse(tileType);

        //creates a new tile and makes a reference for that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //uses newTile to change the position of the tile
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);

        if(tileIndex == 1)
        {
            newTile.IsEmpty = false;
        }
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    // private void SpawnPoints()
    // {
    //     pointASpawn = new Point(0,0);
    //     GameObject tmp =  (GameObject)Instantiate(pointAPrefab, Tiles[pointASpawn].GetComponent<TileScript>().WorldPosition, quaternion.identity);
    //     PointA = tmp.GetComponent<Spawn>();
    //     PointA.name = "PointA";

    //     pointBSpawn = new Point(11,6);
    //     Instantiate(pointBPrefab, Tiles[pointBSpawn].GetComponent<TileScript>().WorldPosition, quaternion.identity);
    // }

    public bool Inbounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    // public void GeneratePath()
    // {
    //     finalPath = AStar.GetPath(pointASpawn, pointBSpawn);
    // }
}
