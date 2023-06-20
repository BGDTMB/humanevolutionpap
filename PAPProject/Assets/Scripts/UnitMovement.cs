using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class UnitMovement : MonoBehaviour
{
    public List<GameObject> btns = new List<GameObject>();
    public int maxMP;
    public int currentMP;
    public GameObject moveTo;
    public HexCell startingCell;
    public Dictionary<HexCell, int> visited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, int> unvisited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, List<HexCell>> pathToEachHex = new Dictionary<HexCell, List<HexCell>>();
    void Start()
    {
        currentMP = maxMP;
    }
    //detects hex unit is currently on
    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<HexCell>() != null)
        {
            if (!unvisited.ContainsKey(collision.GetComponent<HexCell>()))
            {
                startingCell = collision.GetComponent<HexCell>();
                unvisited.Add(startingCell, 0);
                if (!pathToEachHex.ContainsKey(startingCell))
                {
                    // Make path to first hex itself
                    pathToEachHex.Add(startingCell, (new List<HexCell> { startingCell }));
                }
            }
        }
    }
    void Update()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(inputRay, out hit) && hit.transform.GetComponent<UnitMovement>() != null)
            {
                UnitMovement uM = hit.transform.GetComponent<UnitMovement>();
                uM.DijkstrasPathFindingAlgorithm();
            }
        }
    }
    // Run Dijkstra's algorithm to find shortest paths
    public void DijkstrasPathFindingAlgorithm()
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
        // Check if unit can reach with its current mp
        foreach (HexCell hex in visited.Keys)
        {
            if ((Mathf.Abs(visited[hex]) <= currentMP && hex != startingCell && hex.properties.name != "Mountains") || (HexCoordinates.Heuristic(hex, startingCell) == 1 && hex.properties.name != "Mountains"))
            {
                GameObject btn = Instantiate(moveTo, new Vector3(hex.transform.position.x, hex.transform.position.y + 10, hex.transform.position.z), Quaternion.identity);
                btns.Add(btn);
                hex.transform.SetParent(this.gameObject.transform, true);
                btn.transform.SetParent(hex.gameObject.transform, true);
            }
        }
    }
    // Get the shortest path from the starting hex to a given target hex
    public List<HexCell> GetShortestPath(HexCell target)
    {
        return pathToEachHex[target];
    }
}