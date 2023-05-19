using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int MP;
    public HexCell D;
    public List<HexCell> visited = new List<HexCell>();
    public List<HexCell> toVisit = new List<HexCell>();
    public HexCell cheapest;
    void Start()
    {
    }
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
        /*if(!visited.Contains(collision.gameObject.HexCell)) ERRO
        {
            visited.Add(collision.gameObject.HexCell);
        }*/
    }
    /*public int Heuristic(HexCell S, HexCell D)
	{
        int startX = S.coordinates.X;
        int startY = S.coordinates.Y;
        int endX = D.coordinates.X;
        int endY = D.coordinates.Y;
		HexCoordinates startOffset = HexCoordinates.OffsetCoordinates(startX, startY);
		HexCoordinates endOffset = HexCoordinates.OffsetCoordinates(endX, endY);
		(int startQ, int startR, int startS) = HexCoordinates.OffsetToCube(startOffset);
		(int endQ, int endR, int endS) = HexCoordinates.OffsetToCube(endOffset);
		int h = (Mathf.Abs(startQ - endQ) + Mathf.Abs(startR - endR) + Mathf.Abs(startS - endS)) / 2;
		return h;
	}*/
    public void PathFind()
    {
        foreach(HexCell hex in visited)
        {
            ColliderScript currentScript = hex.properties.colliders[0].GetComponent<ColliderScript>();
            for(int x = 0; x < hex.properties.colliders.Count - 1; x++)
            {
                ColliderScript nextScript = hex.properties.colliders[x + 1].GetComponent<ColliderScript>();
                if(nextScript.neighbour.properties.movementCost < currentScript.neighbour.properties.movementCost)
                {
                    currentScript = nextScript;
                    visited.Add(currentScript.neighbour);
                }
            }
        }
    }
}
