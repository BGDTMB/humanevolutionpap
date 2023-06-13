using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class UnitMovement : MonoBehaviour
{
    public int MP;
    public Dictionary<HexCell, int> visited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, int> unvisited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, HexCell> predecessors = new Dictionary<HexCell, HexCell>();
    //detects hex unit is currently on
    void OnTriggerEnter(Collider collision)
    {
        if (!unvisited.ContainsKey(collision.GetComponent<HexCell>()))
        {
            unvisited.Add(collision.GetComponent<HexCell>(), 0);
        }
    }

    // Run Dijkstra's algorithm to find shortest paths
    public void PathFind()
    {
        while (unvisited.Count > 0)
        {
            // Find the cell with the minimum distance in the unvisited dictionary
            HexCell currentCell = unvisited.OrderBy(pair => pair.Value).First().Key;
            int currentDistance = unvisited[currentCell];

            // Mark the current cell as visited and remove it from the unvisited dictionary
            visited[currentCell] = currentDistance;
            unvisited.Remove(currentCell);

            // Visit neighboring cells and update distances
            foreach (BoxCollider coll in currentCell.properties.colliders)
            {
                HexCell neighbor = coll.GetComponent<ColliderScript>().neighbour;
                if (neighbor != null && !visited.ContainsKey(neighbor) && HexCoordinates.Heuristic(neighbor, visited.ElementAt(0).Key) <= 3)
                {
                    int neighborDistance = currentDistance + neighbor.properties.movementCost;
                    if (!unvisited.ContainsKey(neighbor))
                    {
                        unvisited.Add(neighbor, neighborDistance);
                        predecessors[neighbor] = currentCell; // Update the predecessor for the neighbor
                    }
                    else if (neighborDistance < unvisited[neighbor])
                    {
                        unvisited[neighbor] = neighborDistance;
                        predecessors[neighbor] = currentCell; // Update the predecessor for the neighbor
                    }
                }
            }
        }
    }

    // Get the shortest path from the starting hex to a given target hex
    public List<HexCell> GetShortestPath(HexCell target)
    {
        List<HexCell> path = new List<HexCell>();

        // Check if a path exists to the target
        if (!predecessors.ContainsKey(target))
        {
            return path; // Return an empty path
        }

        // Reconstruct the path from the predecessors dictionary
        HexCell currentCell = target;
        while (currentCell != null)
        {
            path.Insert(0, currentCell);
            if (predecessors.ContainsKey(currentCell))
            {
                currentCell = predecessors[currentCell];
            }
            else
            {
                currentCell = null;
            }
        }
        return path;
    }
}
