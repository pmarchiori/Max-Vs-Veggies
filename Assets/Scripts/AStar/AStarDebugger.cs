using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDebugger : MonoBehaviour
{
    [SerializeField] private TileScript start;
    [SerializeField] private TileScript goal;

    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private GameObject debugTilePrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTile();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            AStar.GetPath(start.GridPosition);
        }
    }

    private void ClickTile()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();

                if(tmp != null)
                {
                    if(start == null)
                    {
                        start = tmp;
                        CreateDebugTile(start.WorldPosition, Color.green);
                    }
                    else if(goal == null)
                    {
                        goal = tmp;
                        CreateDebugTile(goal.WorldPosition, Color.red);
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList)
    {
        foreach (Node node in openList)
        {
            if(node.TileReference != start)
            {
                CreateDebugTile(node.TileReference.WorldPosition, Color.cyan, node);
            }

            PointToParent(node, node.TileReference.WorldPosition);
        }

        foreach (Node node in closedList)
        {
            if(node.TileReference != start  && node.TileReference != goal)
            {
                CreateDebugTile(node.TileReference.WorldPosition, Color.blue, node);
            }
        }
    }

    private void PointToParent(Node node, Vector2 position)
    {
        if(node.Parent != null)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;
            
            //right
            if((node.GridPosition.X < node.Parent.GridPosition.X) && 
            (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,0);
            }
            //top right
            else if((node.GridPosition.X < node.Parent.GridPosition.X) && 
            (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,45);
            }
            //up
            else if((node.GridPosition.X == node.Parent.GridPosition.X) && 
            (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,90);
            }
            //top left
            else if((node.GridPosition.X > node.Parent.GridPosition.X) && 
            (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,135);
            }
            //left
            else if((node.GridPosition.X > node.Parent.GridPosition.X) && 
            (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,180);
            }
            //bottom left
            else if((node.GridPosition.X > node.Parent.GridPosition.X) && 
            (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,225);
            }
            //bottom
            else if((node.GridPosition.X == node.Parent.GridPosition.X) && 
            (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,270);
            }
            //bottom right
            else if((node.GridPosition.X < node.Parent.GridPosition.X) && 
            (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0,315);
            }
        }       
    }

    private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null)
    {
        GameObject debugTile = (GameObject)Instantiate(debugTilePrefab, worldPos, Quaternion.identity);

        if(node != null)
        {
            debugTile.GetComponent<DebugTile>().G.text += node.G;
        }

        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}
