using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Point GridPosition {get; private set;}

    public TileScript TileReference {get; private set;} //reference to the tile this node belongs to

    public Node Parent { get; private set; }
    public int G { get; set; }

    public Node(TileScript tileReference)
    {
        this.TileReference = tileReference;
        this.GridPosition = tileReference.GridPosition;
    }

    public void CalcValues(Node parent, int gCost)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
    }
}
