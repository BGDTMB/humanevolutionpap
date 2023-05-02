using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
	[SerializeField]
	private int x, z;

	public int X { get { return -x; } }

	public int Z { get { return -z; } }

	public int Y { get { return -X - Z; } }

	public HexCoordinates(int x, int z)
	{
		this.x = x;
		this.z = z;
	}
	public static HexCoordinates FromOffsetCoordinates(int x, int z)
	{
		return new HexCoordinates(x - z / 2, z);
	}
	public override string ToString()
	{
		return "(" + Y.ToString() + ", " + Z.ToString() + ", " + X.ToString() + ")";
	}

	public string ToStringOnSeparateLines()
	{
		return Y.ToString() + "\n" + Z.ToString() + "\n" + X.ToString();
	}
	public static HexCoordinates FromPosition(Vector3 position)
	{
		float x = position.x / (HexMetrics.innerRadius * 2f);
		float y = -x;
		float offset = position.z / (HexMetrics.outerRadius * 3f);
		x -= offset;
		y -= offset;
		int iX = Mathf.RoundToInt(x);
		int iY = Mathf.RoundToInt(y);
		int iZ = Mathf.RoundToInt(-x - y);

		return new HexCoordinates(iX, iZ);
	}
}
[System.Serializable]
public struct Properties
{
	[SerializeField]
	public string name;
	public int science;
	public int culture;
	public int gold;
	public int food;
	public int production;
	public int movementCost;
	public bool neighbouringCityCenter;
	public bool hasWoods;
	public bool hasRainforest;
	public bool hasMarsh;
	public bool hasOasis;
	public List<BoxCollider> colliders;
	public bool hasStructure;

	public Properties(string name, int science, int culture, int gold, int food, int production, int movementCost, bool neighbouringCityCenter, bool hasWoods, bool hasRainforest, bool hasMarsh, bool hasOasis, List<BoxCollider> colliders, bool hasStructure)
    {
		this.name = name;
		this.science = science;
		this.culture = culture;
		this.gold = gold;
		this.food = food;
		this.production = production;
		this.movementCost = movementCost;
		this.neighbouringCityCenter = neighbouringCityCenter;
		this.hasWoods = hasWoods;
		this.hasRainforest = hasRainforest;
		this.hasMarsh = hasMarsh;
		this.hasOasis = hasOasis;
		this.colliders = colliders;
		this.hasStructure = hasStructure;
    }
}
public class HexCell : MonoBehaviour
{
	public HexCell() { }
	public HexCoordinates coordinates;
	public Properties properties;
	public Color color;
}