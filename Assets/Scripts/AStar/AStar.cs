using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

    public static void GetPath(Point start, Point goal) //generates path with A* algorithm
    {
        if(nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>(); //creates an open list to be used with the A* algoritm
        HashSet<Node> closedList = new HashSet<Node>(); //creates a closed list to be used with the A* algoritm

        Stack<Node>finalPath = new Stack<Node>();

        Node currentNode = nodes[start]; //finds and creates a reference to the start node

        openList.Add(currentNode); //adds the start node to the open list

        while(openList.Count > 0)
        {
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
                    else //scores 14 if the move is diagonal
                    {
                        if(!ConnectedDiagonally(currentNode, nodes[neighbourPosition]))
                        {
                            continue;
                        }
                        gCost = 14;
                    }

                    Node neighbour = nodes[neighbourPosition]; //adds the neighbour to the open list

                    if(openList.Contains(neighbour))
                    {
                        if(currentNode.G + gCost < neighbour.G)
                        {
                            neighbour.CalcValues(currentNode, nodes[goal], gCost); //calculates all values for the neighbour
                        }
                    }
                    else if(!closedList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                        neighbour.CalcValues(currentNode, nodes[goal], gCost);
                    }
                }
            }
        }
        //moves current node from open to closed list
        openList.Remove(currentNode);
        closedList.Add(currentNode);

        if(openList.Count > 0)
        {
            currentNode  = openList.OrderBy(n => n.F).First(); //sorts the list by F value and selects the first
        }

        if(currentNode == nodes[goal])
        {
            while(currentNode.GridPosition != start)
            {
                finalPath.Push(currentNode);
                currentNode = currentNode.Parent;
            }

            break;
        }

        }

        //DEBUG
        GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);
    }

    private static bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        Point direction = neighbour.GridPosition - currentNode.GridPosition;

        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        if(LevelManager.Instance.Inbounds(first) && !LevelManager.Instance.Tiles[first].Walkable)
        {   
            return false;
        }
        if(LevelManager.Instance.Inbounds(second) && !LevelManager.Instance.Tiles[second].Walkable)
        {   
            return false;
        }

        return true;
    }
}
