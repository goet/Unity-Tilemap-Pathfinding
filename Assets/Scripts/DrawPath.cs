using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
/// <summary>
/// Demo class to display the path the pathfinding object found
/// </summary>
public class DrawPath : MonoBehaviour
{
    [SerializeField]
    private Pathfinding pathfinding;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int start = RoundToLogicalPosition(transform.position);
            Vector2Int destination = RoundToLogicalPosition(mousePos);

            Debug.Log("pathfinding started between" + start + " and " + destination);

            if (pathfinding.EligibleClick(destination))
            {
                var path = new List<Node>();
                pathfinding.FindPath(start, destination, out path);
                DisplayPathAsLine(path);
            }
            else
            {
                Debug.Log("Can't make a path to a wall!");
            }
        }
    }

    /// <summary>
    /// Rounds a Vector3 to a point on the grid
    /// </summary>    
    private Vector2Int RoundToLogicalPosition(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }

    /// <summary>
    /// Turns the path into a line with the current position in mind
    /// IMPORTANT: the logical location of a tile is not the same as it's world position, which is why an offset is applied
    /// </summary>
    private void DisplayPathAsLine(List<Node> path)
    {
        lineRenderer.positionCount = path.Count + 1;
        lineRenderer.SetPosition(0, new Vector3(transform.position.x + .5f, transform.position.y + .5f));
        var i = 1;

        foreach (Node node in path)
        {
            Vector2Int logicalPosition = path[i - 1].Position;
            var offset = 0.5f;
            var realPosition = new Vector3(logicalPosition.x + offset, logicalPosition.y + offset);

            lineRenderer.SetPosition(i, realPosition);
            i++;
        }
    }
}
