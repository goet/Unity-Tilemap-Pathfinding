using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// An implementation of A* for the new TileMap system released by Unity
/// </summary>
public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    
    /// <summary>
    /// Simple check to see if the user clicked on a passable wall as our destination
    /// </summary>
    /// <param name="target">The logical position of the tile</param>
    /// <returns></returns>
    public bool EligibleClick(Vector2Int target)
    {
        return tilemap.GetTile<LabyrinthTile>(new Vector3Int(target.x, target.y, 0)).Passable;
    }

    /// <summary>
    /// Looks for a path
    /// </summary>
    /// <param name="start">The starting position</param>
    /// <param name="target">The destination the path should lead to</param>
    /// <param name="path">The path created</param>
    public void FindPath(Vector2Int start, Vector2Int target, out List<Node> path)
    {
        Debug.Log("looking for path");
        path = new List<Node>();
        Node startNode = new Node(start.x, start.y, true);
        Node targetNode = new Node(target.x, target.y, true);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        if (!tilemap.GetTile<LabyrinthTile>(new Vector3Int(targetNode.Position.x, targetNode.Position.y, 0)).Passable)
            return;

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.Equals(targetNode))
            {
                path = RetracePath(startNode, currentNode);
                return;
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.Passable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        openSet = openSet.OrderBy(a => a.fCost).ToList();
                    }
                }
            }

            openSet = openSet.OrderBy(a => a.fCost).ToList();
        }
    }

    private List<Node> RetracePath(Node startNode, Node endNote)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNote;

        while (!currentNode.Equals(startNode))
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        Debug.Log("path found, steps: " + path.Count);

        path.Reverse();
        return path;
    }

    private int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.Position.x - b.Position.x);
        int dstY = Mathf.Abs(a.Position.y - b.Position.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }

    private List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int nx = node.Position.x + x;
                int ny = node.Position.y + y;

                LabyrinthTile current = tilemap.GetTile<LabyrinthTile>(new Vector3Int(nx, ny, 0));

                if (current)
                    neighbours.Add(new Node(nx, ny, current.Passable));
            }
        }

        return neighbours;
    }
}

