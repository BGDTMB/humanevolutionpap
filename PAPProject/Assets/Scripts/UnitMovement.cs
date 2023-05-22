using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int MP;
    public List<HexCell> visited = new List<HexCell>();
    public List<HexCell> toVisit = new List<HexCell>();
    public ColliderScript cheapest;
    public int distance;
    void OnTriggerEnter(Collider collision)
    {
        if(!visited.Contains(collision.GetComponent<HexCell>()))
        {
            visited.Add(collision.GetComponent<HexCell>());
        }
    }
    public void PathFind()
    {
        foreach(HexCell hex in visited)
        {
            cheapest = hex.properties.colliders[0].GetComponent<ColliderScript>();
            for(int x = 0; x < hex.properties.colliders.Count - 1; x++)
            {
                ColliderScript nextScript = hex.properties.colliders[x + 1].GetComponent<ColliderScript>();
                if(nextScript.neighbour.properties.movementCost < cheapest.neighbour.properties.movementCost)
                {
                    cheapest = nextScript;
                }
            }
            visited.Add(cheapest.neighbour);
        }
    }
}
