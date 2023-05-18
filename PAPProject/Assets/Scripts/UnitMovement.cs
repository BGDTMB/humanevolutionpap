using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int MP;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public int Heuristic(HexCell hex, int startX, int startY, int endX, int endY)
	{
		HexCoordinates startOffset = HexCoordinates.OffsetCoordinates(startX, startY);
		HexCoordinates endOffset = HexCoordinates.OffsetCoordinates(endX, endY);
		(int startQ, int startR, int startS) = HexCoordinates.OffsetToCube(startOffset);
		(int endQ, int endR, int endS) = HexCoordinates.OffsetToCube(endOffset);
		int h = (Mathf.Abs(startQ - endQ) + Mathf.Abs(startR - endR) + Mathf.Abs(startS - endS)) / 2;
		return h;
	}
}
