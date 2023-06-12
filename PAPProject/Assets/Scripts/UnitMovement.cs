using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class UnitMovement : MonoBehaviour
{
    public int MP;
    public Dictionary<HexCell, int> visited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, int> unvisited = new Dictionary<HexCell, int>();
    //detects hex unit is currently on
    void OnTriggerEnter(Collider collision)
    {
        if (!unvisited.ContainsKey(collision.GetComponent<HexCell>()))
        {
            unvisited.Add(collision.GetComponent<HexCell>(), 0);
        }
    }
    //uses Dijkstra's algorithm to find shortest path to all hexes in the unit's MP range
    public void PathFind()
    {
        //go through all cells and add only the cells that are in a distance of up to 3 hexes away from current hex to the search group to minimmize use of computer resources
        HexCell C = unvisited.ElementAt(0).Key;
        foreach (HexCell hex in HexGrid.cells)
        {
            if (HexCoordinates.Heuristic(C, hex) < 4 && !unvisited.ContainsKey(hex))
            {
                unvisited.Add(hex, int.MaxValue);
            }
        }
        Dictionary<HexCell, int> tempDictionary = new Dictionary<HexCell, int>();
        foreach (var kvp in unvisited)
        {
            tempDictionary.Add(kvp.Key, kvp.Value);
        }
        var cheapest = tempDictionary.OrderBy(kvp => kvp.Value).First();
        C = cheapest.Key;
        foreach (BoxCollider coll in C.properties.colliders)
        {
            HexCell N = coll.GetComponent<ColliderScript>().neighbour;
            if (C != null && N != null)
            {
                if (unvisited.ContainsKey(C) && unvisited.ContainsKey(N))
                {
                    if (unvisited[C] + unvisited[N] < unvisited[N])
                    {
                        unvisited[N] = unvisited[C] + unvisited[N];
                    }
                    visited.Add(C, unvisited[C]);
                    unvisited.Remove(C);
                }
            }
        }
    }
}