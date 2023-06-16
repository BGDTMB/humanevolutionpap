using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class UnitMovement : MonoBehaviour
{
    public Dictionary<HexCell, int> visited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, int> unvisited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, List<HexCell>> pathToEachHex = new Dictionary<HexCell, List<HexCell>>();
    //detects hex unit is currently on
    void OnTriggerEnter(Collider collision)
    {
        if (!unvisited.ContainsKey(collision.GetComponent<HexCell>()))
        {
            unvisited.Add(collision.GetComponent<HexCell>(), 0);
            if(!pathToEachHex.ContainsKey(collision.GetComponent<HexCell>()))
            {
                // Make path to first hex itself
                pathToEachHex.Add(collision.GetComponent<HexCell>(), new List<HexCell> { collision.GetComponent<HexCell>() });
            }
        }
    }

    // Run Dijkstra's algorithm to find shortest paths
    public void PathFind()
    {
        // Add to unvisited all tiles in 3 tile radius
        foreach (HexCell hex in HexGrid.cells)
        {
            if (unvisited.Count > 0)
            {
                if (!unvisited.ContainsKey(hex) && HexCoordinates.Heuristic(unvisited.ElementAt(0).Key, hex) <= 2)
                {
                    unvisited.Add(hex, int.MaxValue);
                }
            }
        }
        while (unvisited.Count > 0)
        {
            // Find the cell with the minimum distance in the unvisited dictionary
            HexCell currentCell = unvisited.OrderBy(pair => pair.Value).FirstOrDefault().Key;
            int currentDistance = unvisited[currentCell];
            // Mark the current cell as visited and remove it from the unvisited dictionary
            visited[currentCell] = currentDistance;
            unvisited.Remove(currentCell);

            // Visit neighboring cells and update distances
            foreach (BoxCollider coll in currentCell.properties.colliders)
            {
                HexCell neighbor = coll.GetComponent<ColliderScript>().neighbour;
                if (neighbor != null && !visited.ContainsKey(neighbor) && unvisited.ContainsKey(neighbor))
                {
                    int neighborDistance = currentDistance + neighbor.properties.movementCost;
                    if (neighborDistance < unvisited[neighbor])
                    {
                        unvisited[neighbor] = neighborDistance;
                        if (!pathToEachHex.ContainsKey(neighbor))
                        {
                            List<HexCell> tempList = new List<HexCell>(pathToEachHex[currentCell]);
                            tempList.Add(neighbor);
                            pathToEachHex[neighbor] = tempList;
                        }
                        else
                        {
                            List<HexCell> tempList = new List<HexCell>(pathToEachHex[currentCell]);
                            tempList.Add(neighbor);
                            pathToEachHex[neighbor] = tempList;
                        }
                    }
                }
            }
        }
    }

    // Get the shortest path from the starting hex to a given target hex
    public List<HexCell> GetShortestPath(HexCell target)
    {
        return pathToEachHex[target];
    }
}