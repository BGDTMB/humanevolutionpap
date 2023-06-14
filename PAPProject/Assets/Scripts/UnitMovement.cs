using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class UnitMovement : MonoBehaviour
{
    public int MP;
    public Dictionary<HexCell, int> visited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, int> unvisited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, List<HexCell>> pathToEachHex = new Dictionary<HexCell, List<HexCell>>();
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
        for(int i = 0; i < 25; i++)
        {
            // Find the cell with the minimum distance in the unvisited dictionary
            HexCell currentCell = unvisited.OrderBy(pair => pair.Value).First().Key;
            int currentDistance = unvisited[currentCell];

            // Mark the current cell as visited and remove it from the unvisited dictionary
            visited[currentCell] = currentDistance;
            unvisited.Remove(currentCell);

            // Add to unvisited all tiles in 3 tile radius
            foreach (HexCell hex in HexGrid.cells)
            {
                if (!unvisited.ContainsKey(hex) && HexCoordinates.Heuristic(visited.ElementAt(0).Key, hex) <= 3)
                {
                    unvisited.Add(hex, int.MaxValue);
                }
            }

            // Make path to first hex itself
            if (!pathToEachHex.ContainsKey(visited.ElementAt(0).Key))
            {
                pathToEachHex.Add(visited.ElementAt(0).Key, new List<HexCell> { visited.ElementAt(0).Key });
            }

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
                        if(!pathToEachHex.ContainsKey(neighbor))
                        {
                            List<HexCell> tempList = new List<HexCell>();
                            tempList.Add(neighbor);
                            tempList.AddRange(pathToEachHex[currentCell]);
                            pathToEachHex.Add(neighbor, tempList);
                        }
                        else
                        {
                            List<HexCell> tempList = new List<HexCell>();
                            tempList.Add(neighbor);
                            tempList.AddRange(pathToEachHex[currentCell]);
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
