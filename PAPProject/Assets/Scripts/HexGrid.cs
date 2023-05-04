using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexGrid : MonoBehaviour
{
	public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
	public BoxCollider collider;
	HexCell[] cells;
	public TextMeshProUGUI cellLabelPrefab;
	public TextMeshProUGUI currentGoldText;
	public TextMeshProUGUI currentCultureText;
	public TextMeshProUGUI currentScienceText;
	public int currentGold = 0;
	public int currentCulture = 0;
	public int currentScience = 0;
	Canvas gridCanvas;
	HexMesh hexMesh;
	public Color defaultColor = Color.white;
	public Color touchedColor = Color.green;
	public float scale = 10f;
    public int seed = 0;
    public int minTerrainType = 1;
    public int maxTerrainType = 14;
    public float tileSize = 10f;
	public GameObject mountainModel;
	public GameObject castleModel;
	public GameObject theatreSquareModel;
	public GameObject universityModel;
	public GameObject bankModel;
	public GameObject plainsModel;
	public GameObject oceanModel;
	public GameObject lakeModel;
	public GameObject desertModel;
	public GameObject grasslandModel;
	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++)
		{
			for (int x = 0; x < width; x++)
			{
				CreateCell(x, z, i++);
			}
		}
		for (int i = 0; i < cells.Length; i++)
		{
			cells[i].properties = ChooseTerrain(GenerateTerrain(HexCoordinates.FromPosition(cells[i].transform.position).X, HexCoordinates.FromPosition(cells[i].transform.position).Y), cells[i].properties);
			cells[i].properties.hasStructure = false;
		}
	}
	void Start()
	{
		for (int i = 0; i < cells.Length; i++)
		{
			TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
			label.rectTransform.SetParent(gridCanvas.transform, false);
			label.rectTransform.anchoredPosition = new Vector2(cells[i].transform.position.x, cells[i].transform.position.z);
			label.autoSizeTextContainer = true;
			label.fontSize = 3;
			label.text = cells[i].properties.name;

			if (cells[i].properties.name == "Ocean")
			{
				GameObject newOceanModel = Instantiate(oceanModel, new Vector3(cells[i].transform.position.x + 13.29746f, cells[i].transform.position.y -10.6f, cells[i].transform.position.z -21.65f), Quaternion.AngleAxis(90, Vector3.left));
				newOceanModel.transform.localScale = new Vector3(1.11f, 1.15f, 1);
				cells[i].color = Color.blue;
			}
			else if (cells[i].properties.name == "Mountains")
			{
				Instantiate(mountainModel, cells[i].transform.position, Quaternion.identity);
				cells[i].color = new Color(71 / 255.0f, 53 / 255.0f, 53 / 255.0f);
			}
            else if (cells[i].properties.name == "Plains")
            {
				GameObject newPlainsModel = Instantiate(plainsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
				newPlainsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
				cells[i].color = Color.green;
			}
			else if (cells[i].properties.name == "Desert")
            {
				Instantiate(desertModel, new Vector3(cells[i].transform.position.x, cells[i].transform.position.y + 0.2f, cells[i].transform.position.z), Quaternion.identity);
				cells[i].color = new Color(224 / 255.0f, 185 / 255.0f, 100 / 255.0f);
			}
            else if (cells[i].properties.name == "Lake")
            {
				GameObject newLakeModel = Instantiate(lakeModel, new Vector3(cells[i].transform.position.x + 13.29746f, cells[i].transform.position.y - 10.6f, cells[i].transform.position.z - 21.65f), Quaternion.AngleAxis(90, Vector3.left));
				newLakeModel.transform.localScale = new Vector3(1.11f, 1.15f, 1);
				cells[i].color = Color.blue;
			}
			else if (cells[i].properties.name == "Grassland")
			{
				GameObject newGrasslandModel = Instantiate(grasslandModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
				newGrasslandModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
				cells[i].color = Color.green;
			}
		}

		hexMesh.Triangulate(cells);
	}
	public void ChooseBuilding(Vector3 position, int id)
	{
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Y * width + coordinates.Y / 2;
		HexCell cell = cells[-index];
		hexMesh.Triangulate(cells);

		cell.properties = SetBuilding(id, cell.properties);
        switch (id)
        {
			case 1:
                if (!cell.properties.hasStructure)
                {
					Instantiate(castleModel, new Vector3(cell.transform.position.x - 3, cell.transform.position.y - 1, cell.transform.position.z - 6), Quaternion.AngleAxis(90, Vector3.up));
					cell.properties.hasStructure = true;
				}
				break;
			case 2:
				if (!cell.properties.hasStructure)
				{
					Instantiate(theatreSquareModel, cell.transform.position, Quaternion.AngleAxis(180, Vector3.up));
					cell.properties.hasStructure = true;
				}
				break;
			case 3:
				if (!cell.properties.hasStructure)
				{
					Instantiate(universityModel, new Vector3(cell.transform.position.x, cell.transform.position.y - 0.01f, cell.transform.position.z), Quaternion.identity);
					cell.properties.hasStructure = true;
				}
				break;
			case 4:
				if (!cell.properties.hasStructure)
				{
					Instantiate(bankModel, new Vector3(cell.transform.position.x, cell.transform.position.y + 2.5f, cell.transform.position.z), Quaternion.identity);
					cell.properties.hasStructure = true;
				}
				break;
        }
    }
	public HexCell selectCell(Vector3 position)
    {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Y * width + coordinates.Y / 2;
		HexCell cell = cells[-index];
		cell.color = touchedColor;
		hexMesh.Triangulate(cells);
		return cell;
	}
	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		BoxCollider NE = Instantiate(collider, new Vector3(cell.transform.position.x + 5, 0, cell.transform.position.z + 7), Quaternion.identity);
		BoxCollider E = Instantiate(collider, new Vector3(cell.transform.position.x + 9, 0, cell.transform.position.z), Quaternion.identity);
		BoxCollider SE = Instantiate(collider, new Vector3(cell.transform.position.x + 5, 0, cell.transform.position.z + -7), Quaternion.identity);
		BoxCollider SW = Instantiate(collider, new Vector3(cell.transform.position.x - 5, 0, cell.transform.position.z - 7), Quaternion.identity);
		BoxCollider W = Instantiate(collider, new Vector3(cell.transform.position.x - 9, 0, cell.transform.position.z), Quaternion.identity);
		BoxCollider NW = Instantiate(collider, new Vector3(cell.transform.position.x - 5, 0, cell.transform.position.z + 7), Quaternion.identity);
		cell.properties.colliders.Add(NE);
		cell.properties.colliders.Add(E);
		cell.properties.colliders.Add(SE);
		cell.properties.colliders.Add(SW);
		cell.properties.colliders.Add(W);
		cell.properties.colliders.Add(NW);
		NE.transform.SetParent(cell.transform);
		E.transform.SetParent(cell.transform);
		SE.transform.SetParent(cell.transform);
		SW.transform.SetParent(cell.transform);
		W.transform.SetParent(cell.transform);
		NW.transform.SetParent(cell.transform);
	}
	public Properties SetBuilding(int id, Properties properties)
    {
		switch (id)
        {
			case 1:
				properties.name = "City Center";
				properties.science = 1;
				properties.culture = 1;
				properties.gold = 1;
				properties.food = 0;
				properties.production = 0;
				break;
			case 2:
				properties.name = "Theatre Square";
				properties.science = 0;
				properties.culture = 3;
				properties.gold = 0;
				properties.food = 0;
				properties.production = 0;
				break;
			case 3:
				properties.name = "Campus";
				properties.science = 3;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 0;
				properties.production = 0;
				break;
			case 4:
				properties.name = "Commercial Hub";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 3;
				properties.food = 0;
				properties.production = 0;
				break;
		}
		return properties;
    }
	public Properties ChooseTerrain(int id, Properties properties)
    {
        switch (id)
        {
			case 1:
				properties.name = "Grassland";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 2;
				properties.production = 0;
				properties.movementCost = 1;
				break;
			case 2:
				properties.name = "Grassland (Hills)";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 2;
				properties.production = 1;
				properties.movementCost = 2;
				break;
			case 3:
				properties.name = "Plains";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 1;
				properties.production = 1;
				properties.movementCost = 1;
				break;
			case 4:
				properties.name = "Plains (Hills)";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 1;
				properties.production = 2;
				properties.movementCost = 2;
				break;
			case 5:
				properties.name = "Desert";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 0;
				properties.production = 0;
				properties.movementCost = 1;
				break;
			case 6:
				properties.name = "Desert (Hills)";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 0;
				properties.production = 1;
				properties.movementCost = 2;
				break;
			case 7:
				properties.name = "Tundra";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 1;
				properties.production = 0;
				properties.movementCost = 1;
				break;
			case 8:
				properties.name = "Tundra (Hills)";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 1;
				properties.production = 1;
				properties.movementCost = 2;
				break;
			case 9:
				properties.name = "Snow";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 0;
				properties.production = 0;
				properties.movementCost = 1;
				break;
			case 10:
				properties.name = "Snow (Hills)";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 0;
				properties.production = 1;
				properties.movementCost = 2;
				break;
			case 11:
				properties.name = "Coast";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 1;
				properties.food = 1;
				properties.production = 0;
				properties.movementCost = 1;
				break;
			case 12:
				properties.name = "Lake";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 1;
				properties.food = 1;
				properties.production = 0;
				properties.movementCost = 1;
				break;
			case 13:
				properties.name = "Ocean";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 1;
				properties.food = 1;
				properties.production = 0;
				properties.movementCost = 1;
				break;
			case 14:
				properties.name = "Mountains";
				properties.science = 0;
				properties.culture = 0;
				properties.gold = 0;
				properties.food = 0;
				properties.production = 0;
				break;
		}
        return properties;
    }
	public int GenerateTerrain(int x, int y)
    {
		float xCoord = x * tileSize * Mathf.Sqrt(3f);
        float yCoord = (y + x * 0.5f) * tileSize * 1.5f;
        float perlinValue = Mathf.PerlinNoise(xCoord / scale + seed, yCoord / scale + seed);
        int terrainType = Mathf.RoundToInt(Mathf.Lerp(minTerrainType, maxTerrainType, perlinValue));
        return terrainType;
	}
	public void NextTurn()
    {
		for (int i = 0; i < cells.Length; i++)
        {
            foreach (Collider coll in cells[i].properties.colliders)
            {
				ColliderScript script = coll.GetComponent<ColliderScript>();
                if (script.neighbour && script.neighbour.properties.name == "City Center")
                {
					cells[i].properties.neighbouringCityCenter = true;
				}
            }
			if (cells[i].properties.neighbouringCityCenter)
			{
				currentGold += cells[i].properties.gold;
				currentCulture += cells[i].properties.culture;
				currentScience += cells[i].properties.science;
			}
		}
		currentGoldText.text = "Gold: " + currentGold.ToString();
		currentCultureText.text = "Culture: " + currentCulture.ToString();
		currentScienceText.text = "Science: " + currentScience.ToString();
	}
}