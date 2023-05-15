using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexGrid : MonoBehaviour
{
    public int seed;
    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    public BoxCollider collider;
    HexCell[] cells;
    public RawImage resourceImgPrefab;
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
    //Buildings
    public GameObject castleModel;
    public GameObject theatreSquareModel;
    public GameObject universityModel;
    public GameObject bankModel;
    //Unadulterated terrain
    public GameObject mountainModel;
    public GameObject oceanModel;
    //Plains
    public GameObject plainsModel;
    public GameObject plainsWithHillsModel;
    public GameObject plainsWithWoodsModel;
    public GameObject plainsWithHillsAndWoodsModel;
    //Desert
    public GameObject desertModel;
    public GameObject oasisModel;
    public GameObject desertWithHillsModel;
    //Grassland
    public GameObject grasslandModel;
    public GameObject grasslandWithHillsModel;
    public GameObject grasslandWithWoodsModel;
    public GameObject grasslandWithHillsAndWoodsModel;
    //Snow
    public GameObject snowModel;
    public GameObject snowWithHillsModel;
    //Tundra
    public GameObject tundraModel;
    public GameObject tundraWithHillsModel;
    public GameObject tundraWithWoodsModel;
    public GameObject tundraWithHillsAndWoodsModel;
    public Texture Aluminium;
    public Texture Coal;
    public Texture Horses;
    public Texture Iron;
    public Texture Niter;
    public Texture Oil;
    public Texture Uranium;
    public Texture Empty;
    void Awake()
    {
        seed = Random.Range(0, 1000000);

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
            cells[i].properties = ChooseTerrain(GenerateTerrain(HexCoordinates.FromIndex(i, width).X, HexCoordinates.FromIndex(i, width).Y), cells[i].properties);
            cells[i].properties.hasStructure = false;
			cells[i].properties.hasOasis = AssignTerrainType(cells[i].properties.name).hasOasis;
			cells[i].properties.hasWoods = AssignTerrainType(cells[i].properties.name).hasWoods;
            cells[i].properties.hasHills = AssignTerrainType(cells[i].properties.name).hasHills;
        }
    }
    void Start()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            RawImage newResourceImgPrefab = Instantiate<RawImage>(resourceImgPrefab);
            newResourceImgPrefab.rectTransform.SetParent(gridCanvas.transform, false);
            newResourceImgPrefab.rectTransform.anchoredPosition = new Vector2(cells[i].transform.position.x, cells[i].transform.position.z);
            float rand = Random.value;
            if (cells[i].properties.name == "Ocean")
            {
                GameObject newOceanModel = Instantiate(oceanModel, new Vector3(cells[i].transform.position.x + 13.29746f, cells[i].transform.position.y - 10.6f, cells[i].transform.position.z - 21.65f), Quaternion.AngleAxis(90, Vector3.left));
                newOceanModel.transform.localScale = new Vector3(1.11f, 1.15f, 1);
                if(rand < 0.12f)
                {
                    cells[i].properties.hasOil = true;
                    newResourceImgPrefab.texture = Oil;
                }
                else
                {
                    newResourceImgPrefab.texture = Empty;
                }
                cells[i].color = Color.blue;
                
            }
            else if (cells[i].properties.name == "Mountains")
            {
                Instantiate(mountainModel, new Vector3(cells[i].transform.position.x + 2.4f, cells[i].transform.position.y - 1f, cells[i].transform.position.z + 1f), Quaternion.identity);
                newResourceImgPrefab.texture = Empty;
                cells[i].color = new Color(71 / 255.0f, 53 / 255.0f, 53 / 255.0f);
            }
            else if (cells[i].properties.name == "Plains")
            {
                if(!cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newPlainsModel = Instantiate(plainsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.12)
                    {
                        cells[i].properties.hasHorses = true;
                        newResourceImgPrefab.texture = Horses;
                    }
                    else if(rand < 0.24)
                    {
                        cells[i].properties.hasNiter = true;
                        newResourceImgPrefab.texture = Niter;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasAluminium = true;
                        newResourceImgPrefab.texture = Aluminium;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if(cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newPlainsWithHillsModel = Instantiate(plainsWithHillsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsWithHillsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if(cells[i].properties.hasWoods && !cells[i].properties.hasHills)
                {
                    GameObject newPlainsWithWoodssModel = Instantiate(plainsWithWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsWithWoodssModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.12)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if(cells[i].properties.hasHills && cells[i].properties.hasWoods)
                {
                    GameObject newPlainsWithHillsAndWoodsModel = Instantiate(plainsWithHillsAndWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsWithHillsAndWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
            }
            else if (cells[i].properties.name == "Desert")
            {
                if(cells[i].properties.hasHills)
                {
                    Instantiate(desertWithHillsModel, new Vector3(cells[i].transform.position.x, cells[i].transform.position.y + 0.2f, cells[i].transform.position.z), Quaternion.identity);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasAluminium = true;
                        newResourceImgPrefab.texture = Aluminium;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = new Color(224 / 255.0f, 185 / 255.0f, 100 / 255.0f);
                }
                else
                {
                    if(cells[i].properties.hasOasis)
                    {
                        GameObject newOasisModel = Instantiate(oasisModel, new Vector3(cells[i].transform.position.x + 1, cells[i].transform.position.y + 1f, cells[i].transform.position.z + 1.3f), Quaternion.identity);
                        newOasisModel.transform.localScale = new Vector3(newOasisModel.transform.localScale.x - 0.1f, newOasisModel.transform.localScale.y, newOasisModel.transform.localScale.z - 0.2f);
                        newResourceImgPrefab.texture = Empty;
                        cells[i].color = new Color(224 / 255.0f, 185 / 255.0f, 100 / 255.0f);
                    }
                    else
                    {
                        Instantiate(desertModel, new Vector3(cells[i].transform.position.x, cells[i].transform.position.y + 0.2f, cells[i].transform.position.z), Quaternion.identity);
                        if(rand < 0.12)
                        {
                            cells[i].properties.hasNiter = true;
                            newResourceImgPrefab.texture = Niter;
                        }
                        else if(rand < 0.24)
                        {
                            cells[i].properties.hasOil = true;
                            newResourceImgPrefab.texture = Oil;
                        }
                        else if(rand < 0.36)
                        {
                            cells[i].properties.hasAluminium = true;
                            newResourceImgPrefab.texture = Aluminium;
                        }
                        else if(rand < 0.48)
                        {
                            cells[i].properties.hasUranium = true;
                            newResourceImgPrefab.texture = Uranium;
                        }
                        else
                        {
                            newResourceImgPrefab.texture = Empty;
                        }
                        cells[i].color = new Color(224 / 255.0f, 185 / 255.0f, 100 / 255.0f);
                    }
                    
                }
            }
            else if (cells[i].properties.name == "Grassland")
            {
                if(!cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newGrasslandModel = Instantiate(grasslandModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasHorses = true;
                        newResourceImgPrefab.texture = Horses;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasNiter = true;
                        newResourceImgPrefab.texture = Niter;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if(cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newGrasslandWithHillsModel = Instantiate(grasslandWithHillsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandWithHillsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if(cells[i].properties.hasWoods && !cells[i].properties.hasHills)
                {
                    GameObject newGrasslandWithWoodsModel = Instantiate(grasslandWithWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandWithWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.12)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if(cells[i].properties.hasHills && cells[i].properties.hasWoods)
                {
                    GameObject newGrasslandWithHillsAndWoodsModel = Instantiate(grasslandWithHillsAndWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandWithHillsAndWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
            }
            else if (cells[i].properties.name == "Snow")
            {
                if(!cells[i].properties.hasHills)
                {
                    GameObject newSnowModel = Instantiate(snowModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newSnowModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.12)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
                else
                {
                    GameObject newSnowWithHillsModel = Instantiate(snowWithHillsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newSnowWithHillsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.12)
                    {
                        cells[i].properties.hasOil = true;
                        newResourceImgPrefab.texture = Oil;
                    }
                    else if(rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
            }
            else if (cells[i].properties.name == "Tundra")
            {
                if(!cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newTundraModel = Instantiate(tundraModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasNiter = true;
                        newResourceImgPrefab.texture = Niter;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasOil = true;
                        newResourceImgPrefab.texture = Oil;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
                else if(cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newTundraWithHillsModel = Instantiate(tundraWithHillsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraWithHillsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
                else if(cells[i].properties.hasWoods && !cells[i].properties.hasHills)
                {
                    GameObject newTundraWithWoodsModel = Instantiate(tundraWithWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraWithWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.12)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
                else if(cells[i].properties.hasHills && cells[i].properties.hasWoods)
                {
                    GameObject newTundraWithHillsAndWoodsModel = Instantiate(tundraWithHillsAndWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraWithHillsAndWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if(rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if(rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if(rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
            }
        }

        hexMesh.Triangulate(cells);
    }
    public void ChooseBuilding(int x, int y, int id)
    {
        int index = width * y + x;
        HexCell cell = cells[index];
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
    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.OffsetCoordinates(x, z);
        cell.properties.heurisitc = HexCoordinates.Heuristic(cell, cell.coordinates.X, cell.coordinates.Y);
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
                properties.name = "Plains";
                properties.science = 0;
                properties.culture = 0;
                properties.gold = 0;
                properties.food = 1;
                properties.production = 1;
                properties.movementCost = 1;
                break;
            case 3:
                properties.name = "Desert";
                properties.science = 0;
                properties.culture = 0;
                properties.gold = 0;
                properties.food = 0;
                properties.production = 0;
                properties.movementCost = 1;
                break;
            case 4:
                properties.name = "Mountains";
                properties.science = 0;
                properties.culture = 0;
                properties.gold = 0;
                properties.food = 0;
                properties.production = 0;
                break;
            case 5:
                properties.name = "Ocean";
                properties.science = 0;
                properties.culture = 0;
                properties.gold = 1;
                properties.food = 1;
                properties.production = 0;
                properties.movementCost = 1;
                break;
            case 6:
                properties.name = "Tundra";
                properties.science = 0;
                properties.culture = 0;
                properties.gold = 0;
                properties.food = 1;
                properties.production = 0;
                properties.movementCost = 1;
                break;
            case 7:
                properties.name = "Snow";
                properties.science = 0;
                properties.culture = 0;
                properties.gold = 0;
                properties.food = 0;
                properties.production = 0;
                properties.movementCost = 1;
                break;
        }
        return properties;
    }
    public int GenerateTerrain(int x, int y)
    {
        float noiseValue = Mathf.PerlinNoise(x * scale + seed, y * scale + seed);
        int terrainType = 0;
        bool closeToPole = false;
        if(y < 2 || y > height - 3)
        {
            closeToPole = true;
        }
        else
        {
            closeToPole = false;
        }

        if(!closeToPole)
        {
            if (noiseValue < 0.2f)
            {
                terrainType = 1;
            }
            else if (noiseValue < 0.4f)
            {
                terrainType = 2;
            }
            else if (noiseValue < 0.6f)
            {
                terrainType = 3;
            }
            else if (noiseValue < 0.8f)
            {
                terrainType = 4;
            }
            else
            {
                terrainType = 5;
            }
        }
        else
        {
            if(noiseValue < 0.33f)
            {
                terrainType = 6;
            }
            else if(noiseValue < 0.66f)
            {
                terrainType = 7;
            }
            else
            {
                terrainType = 5;
            }
        }
        return terrainType;
    }
	public (bool hasOasis, bool hasWoods, bool hasHills) AssignTerrainType(string terrainName)
	{
		bool hasOasis = false;
		bool hasWoods = false;
        bool hasHills = false;
        float rand1 = Random.value;
        float rand2 = Random.value;
		switch(terrainName)
		{
			case "Plains":
			case "Grassland":
			case "Tundra":
				if(rand1 < 0.5f)
				{
					hasWoods = true;
                    if(rand2 < 0.5f)
				    {
				    	hasHills = true;
				    }
				    else
				    {
				    	hasHills = false;
				    }
				}
				else
				{
					hasWoods = false;
                    if(rand2 < 0.5f)
			    	{
			    		hasHills = true;
			    	}
			    	else
			    	{
			    		hasHills = false;
			    	}
				}
				break;
			case "Desert":
                if(rand1 < 0.5f)
				{
					hasHills = true;
				}
				else
				{
					hasHills = false;
                    if(rand2 < 0.5f)
				    {
				    	hasOasis = true;
				    }
				    else
				    {
				    	hasOasis = false;
				    }
				}
				break;
            case "Snow":
                if(rand1 < 0.5f)
                {
                    hasHills = true;
                }
                else
                {
                    hasHills = false;
                }
                break;
		}
		return (hasOasis, hasWoods, hasHills);
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