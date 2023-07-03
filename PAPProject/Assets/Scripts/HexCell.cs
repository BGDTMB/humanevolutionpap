using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
	[SerializeField]
	private int x, y;

	public int X { get { return x; } }

	public int Y { get { return y; } }

	public HexCoordinates(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	public static HexCoordinates OffsetCoordinates(int x, int y)
	{
		return new HexCoordinates(x, y);
	}
	public override string ToString()
	{
		return "(" + X.ToString() + ", " + Y.ToString() + ")";
	}
	public static HexCoordinates FromPosition(Vector3 position)
	{
		float x = position.x / (HexMetrics.innerRadius * 2f);
		float y = -x;
		float offset = position.y / (HexMetrics.outerRadius * 3f);
		x -= offset;
		y -= offset;
		int iX = Mathf.RoundToInt(x);
		int iY = Mathf.RoundToInt(-x - y);

		return new HexCoordinates(iX, iY);
	}
	public static HexCoordinates FromIndex(int index, int width)
	{
    	int y = index / width;
    	int x = index % width;

   		return new HexCoordinates(x, y);
	}
	public static int FromCoordinates(HexCoordinates coords, int columns)
	{
		int index = columns * coords.Y + coords.X;
		return index;
	}
	public static (int q, int r, int s) OffsetToCube(HexCoordinates offset)
	{
		int q = offset.x - (offset.y - (offset.y & 1)) / 2;
    	int r = offset.y;
		int s = -q-r;
    	return (q, r, s);
	}
	public static int Heuristic(HexCell S, HexCell D)
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
	}
}
[System.Serializable]
public struct Properties
{
	[SerializeField]
	public string name;
	public Dictionary<string, int> yields;
	public int movementCost;
	public bool ownedByCity;
	public bool hasWoods;
	public bool hasOasis;
	public bool hasHills;
	public bool hasStructure;
	public bool hasHorses;
	public bool hasIron;
	public bool hasNiter;
	public bool hasCoal;
	public bool hasOil;
	public bool hasAluminium;
	public bool hasUranium;
	public int heuristicNearestCityCenter;
	public List<BoxCollider> colliders;
	public int howManyYields;
	public int cost;
	public bool purchasable;
	public GameObject enemy;

	public Properties(string name, Dictionary<string, int> yields, int movementCost, bool ownedByCity, 
	bool hasWoods, bool hasOasis, bool hasHills, bool hasStructure, bool hasHorses,
	bool hasIron, bool hasNiter, bool hasCoal, bool hasOil, bool hasAluminium, bool hasUranium,
	int heuristicNearestCityCenter, List<BoxCollider> colliders, int howManyYields, int cost, bool purchasable, GameObject enemy)
    {
		this.name = name;
		this.yields = yields;
		this.movementCost = movementCost;
		this.ownedByCity = ownedByCity;
		this.hasWoods = hasWoods;
		this.hasOasis = hasOasis;
		this.hasHills = hasHills;
		this.hasStructure = hasStructure;
		this.hasHorses = hasHorses;
		this.hasIron = hasIron;
		this.hasNiter = hasNiter;
		this.hasCoal = hasCoal;
		this.hasOil = hasOil;
		this.hasAluminium = hasAluminium;
		this.hasUranium = hasUranium;
		this.heuristicNearestCityCenter = heuristicNearestCityCenter;
		this.colliders = colliders;
		this.howManyYields = howManyYields;
		this.cost = cost;
		this.purchasable = purchasable;
		this.enemy = enemy;
    }
}
public class HexCell : MonoBehaviour
{
	public HexCell() { }
	public HexCoordinates coordinates;
	public Properties properties;
	public Color color;
}