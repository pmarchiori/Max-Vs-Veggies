using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class AStar
{   
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes() //creates a node for each tile
    {
        nodes = new Dictionary<Point, Node>(); //instantiate dictionary

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values) //run through all the game tiles
        {
            nodes.Add(tile.GridPosition, new Node(tile)); //adds the node to the dictionary
        }   
    }

    public static void GetPath(Point start)
    {
        if(nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>(); //creates an open list to be used with the A* algoritm
        HashSet<Node> closedList = new HashSet<Node>(); //creates a closed list to be used with the A* algoritm

        Node currentNode = nodes[start]; //finds and creates a reference to the start node

        openList.Add(currentNode); //adds the start node to the open list

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Point neighbourPosition = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                
                if(LevelManager.Instance.Inbounds(neighbourPosition) && 
                LevelManager.Instance.Tiles[neighbourPosition].Walkable && 
                neighbourPosition != currentNode.GridPosition)
                {
                    int gCost = 0;

                    if(Math.Abs(x-y) == 1)
                    {
                        gCost = 10;
                    }
                    else
                    {
                        gCost = 14;
                    }

                    Node neighbour = nodes[neighbourPosition]; //adds the neighbour to the open list

                    if(!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }

                    neighbour.CalcValues(currentNode, gCost); //calculates all values for the neighbour
                }
            }
        }

        openList.Remove(currentNode);
        closedList.Add(currentNode);

        //DEBUG
        GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList);
    }
}
