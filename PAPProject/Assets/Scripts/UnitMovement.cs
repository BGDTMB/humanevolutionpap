using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class Extension
{
    public static K FindKeyByValue<K, V>(this Dictionary<K, V> dict, V value)
    {
        Dictionary<V, K> revDict = dict.ToDictionary(pair => pair.Value, pair => pair.Key);
        return revDict[value];
    }
}
public class UnitMovement : MonoBehaviour
{
    //largest number int can hold, for the purposes of this script it represents infinity
    public int inf = 2147483647;
    public int MP;
    public Dictionary<HexCell, int> visited = new Dictionary<HexCell, int>();
    public Dictionary<HexCell, int> unvisited = new Dictionary<HexCell, int>();
    void Start()
    {
        PathFind();
    }
    //detects hex unit is currently on
    void OnTriggerEnter(Collider collision)
    {
        if (!visited.ContainsKey(collision.GetComponent<HexCell>()))
        {
            visited.Add(collision.GetComponent<HexCell>(), 0);
        }
    }
    //uses Dijkstra's algorithm to find shortest path to all hexes in the unit's MP range
    public void PathFind()
    {
        foreach (HexCell hex in HexGrid.cells)
        {
            if (HexCoordinates.Heuristic(visited.ElementAt(0).Key, hex) < 4 && !unvisited.ContainsKey(hex))
            {
                unvisited.Add(hex, inf);
            }
        }
        for (int x = 0; x < unvisited.Count; x++)
        {
            for (int y = 0; y < visited.ElementAt(x).Key.properties.colliders.Count; y++)
            {
                HexCell neighbour = visited.ElementAt(x).Key.properties.colliders[y].GetComponent<ColliderScript>().neighbour;
                HexCell cheapest = visited.ElementAt(0).Key;
                if (visited.ElementAt(x).Value + neighbour.properties.movementCost < unvisited.ElementAt(x).Value)
                {
                    unvisited[unvisited.ElementAt(x).Key] = visited.ElementAt(x).Value + neighbour.properties.movementCost;
                }
                foreach (KeyValuePair<HexCell, int> kvp in unvisited)
                {
                    if (kvp.Value < unvisited[cheapest])
                    {
                        cheapest = Extension.FindKeyByValue(unvisited, kvp.Value);
                    }
                    Debug.Log(cheapest);
                }
            }
        }
    }
}