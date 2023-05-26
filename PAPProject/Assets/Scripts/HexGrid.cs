using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexGrid : MonoBehaviour
{
    public int seed;
    public static int width = 13;
    public int height = 13;
    public static bool inMenu;
    public HexCell cellPrefab;
    public BoxCollider collider;
    public static HexCell[] cells;
    public RawImage resourceImgPrefab;
    public RawImage yieldImgPrefab;
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
    public GameObject cityCenter;
    //Buildings Models
    public GameObject cityCenterModel;
    public GameObject theatreSquareModel;
    public GameObject campusModel;
    public GameObject commercialHubModel;
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
    //Resources
    public Texture Aluminium;
    public Texture Coal;
    public Texture Horses;
    public Texture Iron;
    public Texture Niter;
    public Texture Oil;
    public Texture Uranium;
    //Yields
    public Texture Science;
    public Texture Culture;
    public Texture Gold;
    public Texture Food;
    public Texture Production;
    //Yields pos
    public Vector3 yieldPosOne = new Vector3(-6, 0, -2);
    public Vector3 yieldPosTwo = new Vector3(-4, 0, -5);
    public Vector3 yieldPosThree = new Vector3(0, 0, -7);
    public Vector3 yieldPosFour = new Vector3(4, 0, -5);
    public Vector3 yieldPosFive = new Vector3(6, 0, -2);
    public Texture Empty;
    void Awake()
    {
        seed = Random.Range(0, 1000000);

        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        //creates map array
        cells = new HexCell[height * width];
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
        //assigns properties of each tile using perlin noise
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
        //adds hex models and resource images to each cell and adds yield values depending on it
        for (int i = 0; i < cells.Length; i++)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// BURRO DO CARALHO PÁ, ATÉ PARECES O ANDRÉ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            RawImage newResourceImgPrefab = Instantiate<RawImage>(resourceImgPrefab);/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            newResourceImgPrefab.rectTransform.SetParent(gridCanvas.transform, false);////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            newResourceImgPrefab.rectTransform.anchoredPosition = new Vector2(cells[i].transform.position.x, cells[i].transform.position.z);//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            int science = 0;
            int culture = 0;
            int gold = 0;
            int food = 0;
            int production = 0;
            float rand = Random.value;
            if (cells[i].properties.name == "Ocean")
            {
                GameObject newOceanModel = Instantiate(oceanModel, new Vector3(cells[i].transform.position.x + 13.29746f, cells[i].transform.position.y - 10.6f, cells[i].transform.position.z - 21.65f), Quaternion.AngleAxis(90, Vector3.left));
                newOceanModel.transform.localScale = new Vector3(1.11f, 1.15f, 1);
                if (rand < 0.12f)
                {
                    cells[i].properties.hasOil = true;
                    production += 3;
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
                if (!cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newPlainsModel = Instantiate(plainsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if (rand < 0.12)
                    {
                        cells[i].properties.hasHorses = true;
                        food += 1;
                        production += 1;
                        newResourceImgPrefab.texture = Horses;
                    }
                    else if (rand < 0.24)
                    {
                        cells[i].properties.hasNiter = true;
                        food += 1;
                        production += 1;
                        newResourceImgPrefab.texture = Niter;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasAluminium = true;
                        science += 1;
                        newResourceImgPrefab.texture = Aluminium;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if (cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newPlainsWithHillsModel = Instantiate(plainsWithHillsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsWithHillsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        science += 1;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if (cells[i].properties.hasWoods && !cells[i].properties.hasHills)
                {
                    GameObject newPlainsWithWoodssModel = Instantiate(plainsWithWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsWithWoodssModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.12)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if (cells[i].properties.hasHills && cells[i].properties.hasWoods)
                {
                    GameObject newPlainsWithHillsAndWoodsModel = Instantiate(plainsWithHillsAndWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newPlainsWithHillsAndWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 3;
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        science += 1;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
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
                if (cells[i].properties.hasHills)
                {
                    Instantiate(desertWithHillsModel, new Vector3(cells[i].transform.position.x, cells[i].transform.position.y + 0.2f, cells[i].transform.position.z), Quaternion.identity);
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        science += 1;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasAluminium = true;
                        science += 1;
                        newResourceImgPrefab.texture = Aluminium;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
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
                    if (cells[i].properties.hasOasis)
                    {
                        GameObject newOasisModel = Instantiate(oasisModel, new Vector3(cells[i].transform.position.x + 1, cells[i].transform.position.y + 1f, cells[i].transform.position.z + 1.3f), Quaternion.identity);
                        newOasisModel.transform.localScale = new Vector3(newOasisModel.transform.localScale.x - 0.1f, newOasisModel.transform.localScale.y, newOasisModel.transform.localScale.z - 0.2f);
                        newResourceImgPrefab.texture = Empty;
                        cells[i].color = new Color(224 / 255.0f, 185 / 255.0f, 100 / 255.0f);
                    }
                    else
                    {
                        Instantiate(desertModel, new Vector3(cells[i].transform.position.x, cells[i].transform.position.y + 0.2f, cells[i].transform.position.z), Quaternion.identity);
                        if (rand < 0.12)
                        {
                            cells[i].properties.hasNiter = true;
                            food += 1;
                            production += 1;
                            newResourceImgPrefab.texture = Niter;
                        }
                        else if (rand < 0.24)
                        {
                            cells[i].properties.hasOil = true;
                            production += 3;
                            newResourceImgPrefab.texture = Oil;
                        }
                        else if (rand < 0.36)
                        {
                            cells[i].properties.hasAluminium = true;
                            science += 1;
                            newResourceImgPrefab.texture = Aluminium;
                        }
                        else if (rand < 0.48)
                        {
                            cells[i].properties.hasUranium = true;
                            production += 2;
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
                if (!cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newGrasslandModel = Instantiate(grasslandModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasHorses = true;
                        food += 1;
                        production += 1;
                        newResourceImgPrefab.texture = Horses;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasNiter = true;
                        food += 1;
                        production += 1;
                        newResourceImgPrefab.texture = Niter;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if (cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newGrasslandWithHillsModel = Instantiate(grasslandWithHillsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandWithHillsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        science += 1;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if (cells[i].properties.hasWoods && !cells[i].properties.hasHills)
                {
                    GameObject newGrasslandWithWoodsModel = Instantiate(grasslandWithWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandWithWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.12)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                    cells[i].color = Color.green;
                }
                else if (cells[i].properties.hasHills && cells[i].properties.hasWoods)
                {
                    GameObject newGrasslandWithHillsAndWoodsModel = Instantiate(grasslandWithHillsAndWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newGrasslandWithHillsAndWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 3;
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        science += 1;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
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
                if (!cells[i].properties.hasHills)
                {
                    GameObject newSnowModel = Instantiate(snowModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newSnowModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if (rand < 0.12)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
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
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.12)
                    {
                        cells[i].properties.hasOil = true;
                        production += 3;
                        newResourceImgPrefab.texture = Oil;
                    }
                    else if (rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
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
                if (!cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newTundraModel = Instantiate(tundraModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasNiter = true;
                        food += 1;
                        production += 1;
                        newResourceImgPrefab.texture = Niter;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasOil = true;
                        production += 3;
                        newResourceImgPrefab.texture = Oil;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
                else if (cells[i].properties.hasHills && !cells[i].properties.hasWoods)
                {
                    GameObject newTundraWithHillsModel = Instantiate(tundraWithHillsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraWithHillsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        science += 1;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
                else if (cells[i].properties.hasWoods && !cells[i].properties.hasHills)
                {
                    GameObject newTundraWithWoodsModel = Instantiate(tundraWithWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraWithWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 2;
                    if (rand < 0.12)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.24)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
                else if (cells[i].properties.hasHills && cells[i].properties.hasWoods)
                {
                    GameObject newTundraWithHillsAndWoodsModel = Instantiate(tundraWithHillsAndWoodsModel, new Vector3(cells[i].transform.position.x + 20.50f, cells[i].transform.position.y + 0.34f, cells[i].transform.position.z + 18.9f), Quaternion.identity);
                    newTundraWithHillsAndWoodsModel.transform.localScale = new Vector3(1.15f, 1, 1.1f);
                    cells[i].properties.movementCost = 3;
                    if (rand < 0.24)
                    {
                        cells[i].properties.hasIron = true;
                        science += 1;
                        newResourceImgPrefab.texture = Iron;
                    }
                    else if (rand < 0.36)
                    {
                        cells[i].properties.hasCoal = true;
                        production += 2;
                        newResourceImgPrefab.texture = Coal;
                    }
                    else if (rand < 0.48)
                    {
                        cells[i].properties.hasUranium = true;
                        production += 2;
                        newResourceImgPrefab.texture = Uranium;
                    }
                    else
                    {
                        newResourceImgPrefab.texture = Empty;
                    }
                }
            }
            cells[i].properties.yields["science"] = science;
            cells[i].properties.yields["culture"] = culture;
            cells[i].properties.yields["gold"] = gold;
            cells[i].properties.yields["food"] = food;
            cells[i].properties.yields["production"] = production;
            foreach (string key in cells[i].properties.yields.Keys)
            {
                if (cells[i].properties.yields[key] != 0)
                {
                    cells[i].properties.howManyYields += 1;
                }
            }
            switch (cells[i].properties.howManyYields)
            {
                case 1:
                    RawImage newYieldImgPrefabOne = Instantiate(yieldImgPrefab, new Vector3(cells[i].transform.position.x, cells[i].transform.position.z, cells[i].transform.position.y) + yieldPosThree, Quaternion.identity);
                    newYieldImgPrefabOne.rectTransform.SetParent(gridCanvas.transform, false);
                    newYieldImgPrefabOne.texture = Coal;
                    break;
                case 2:
                    RawImage newYieldImgPrefabTwo = Instantiate(yieldImgPrefab, new Vector3(cells[i].transform.position.x, cells[i].transform.position.z, cells[i].transform.position.y) + yieldPosThree, Quaternion.identity);
                    newYieldImgPrefabTwo.texture = Coal;
                    newYieldImgPrefabTwo.rectTransform.SetParent(gridCanvas.transform, false);
                    break;
                case 3:
                    RawImage newYieldImgPrefabThree = Instantiate(yieldImgPrefab, new Vector3(cells[i].transform.position.x, cells[i].transform.position.z, cells[i].transform.position.y) + yieldPosThree, Quaternion.identity);
                    newYieldImgPrefabThree.rectTransform.SetParent(gridCanvas.transform, false);
                    newYieldImgPrefabThree.texture = Coal;
                    break;
                case 4:
                    RawImage newYieldImgPrefabFour = Instantiate(yieldImgPrefab, new Vector3(cells[i].transform.position.x, cells[i].transform.position.z, cells[i].transform.position.y) + yieldPosThree, Quaternion.identity);
                    newYieldImgPrefabFour.rectTransform.SetParent(gridCanvas.transform, false);
                    newYieldImgPrefabFour.texture = Coal;
                    break;
                case 5:
                    RawImage newYieldImgPrefabFive = Instantiate(yieldImgPrefab, new Vector3(cells[i].transform.position.x, cells[i].transform.position.z, cells[i].transform.position.y) + yieldPosThree, Quaternion.identity);
                    newYieldImgPrefabFive.rectTransform.SetParent(gridCanvas.transform, false);
                    newYieldImgPrefabFive.texture = Coal;
                    break;
            }
        }
        hexMesh.Triangulate(cells);
    }
    //selects building model to be placed
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
                    Instantiate(cityCenterModel, new Vector3(cell.transform.position.x - 3, cell.transform.position.y - 1, cell.transform.position.z - 6), Quaternion.AngleAxis(90, Vector3.up));
                    GameObject cc = Instantiate(cityCenter, cell.transform.position, Quaternion.identity);
                    cc.transform.SetParent(cell.gameObject.transform, true);
                    cell.properties.hasStructure = true;
                }
                break;
            case 2:
                if (!cell.properties.hasStructure && cell.properties.neighbouringCityCenter)
                {
                    Instantiate(theatreSquareModel, cell.transform.position, Quaternion.AngleAxis(180, Vector3.up));
                    cell.properties.hasStructure = true;
                }
                break;
            case 3:
                if (!cell.properties.hasStructure && cell.properties.neighbouringCityCenter)
                {
                    Instantiate(campusModel, new Vector3(cell.transform.position.x, cell.transform.position.y - 0.01f, cell.transform.position.z), Quaternion.identity);
                    cell.properties.hasStructure = true;
                }
                break;
            case 4:
                if (!cell.properties.hasStructure && cell.properties.neighbouringCityCenter)
                {
                    Instantiate(commercialHubModel, new Vector3(cell.transform.position.x, cell.transform.position.y + 2.5f, cell.transform.position.z), Quaternion.identity);
                    cell.properties.hasStructure = true;
                }
                break;
        }
    }
    void CreateCell(int x, int z, int i)
    {
        //converts data from loops to 'real world' position
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        //creates the cell and adds to array
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        //converts 'real world' position to offset coords
        cell.coordinates = HexCoordinates.OffsetCoordinates(x, z);
        cell.color = defaultColor;

        //add boxcolliders to all sides as children of the hex and add them to a colliders list
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

        //adds to yields dictionary all possible yield options
        cell.properties.yields = new Dictionary<string, int>();
        cell.properties.yields.Add("science", 0);
        cell.properties.yields.Add("culture", 0);
        cell.properties.yields.Add("gold", 0);
        cell.properties.yields.Add("food", 0);
        cell.properties.yields.Add("production", 0);
    }
    //sets properties to hex depending on building
    public Properties SetBuilding(int id, Properties properties)
    {
        switch (id)
        {
            case 1:
                properties.name = "City Center";
                properties.yields["science"] = 1;
                properties.yields["culture"] = 1;
                properties.yields["gold"] = 1;
                properties.yields["food"] = 0;
                properties.yields["production"] = 0;
                break;
            case 2:
                properties.name = "Theatre Square";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 3;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 0;
                properties.yields["production"] = 0;
                break;
            case 3:
                properties.name = "Campus";
                properties.yields["science"] = 3;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 0;
                properties.yields["production"] = 0;
                break;
            case 4:
                properties.name = "Commercial Hub";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 3;
                properties.yields["food"] = 0;
                properties.yields["production"] = 0;
                break;
        }
        return properties;
    }
    //terrain types and its properties
    public Properties ChooseTerrain(int id, Properties properties)
    {
        switch (id)
        {
            case 1:
                properties.name = "Grassland";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 2;
                properties.yields["production"] = 0;
                properties.movementCost = 1;
                break;
            case 2:
                properties.name = "Plains";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 1;
                properties.yields["production"] = 1;
                properties.movementCost = 1;
                break;
            case 3:
                properties.name = "Desert";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 0;
                properties.yields["production"] = 0;
                properties.movementCost = 1;
                break;
            case 4:
                properties.name = "Mountains";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 0;
                properties.yields["production"] = 0;
                break;
            case 5:
                properties.name = "Ocean";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 1;
                properties.yields["food"] = 1;
                properties.yields["production"] = 0;
                properties.movementCost = 1;
                break;
            case 6:
                properties.name = "Tundra";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 1;
                properties.yields["production"] = 0;
                properties.movementCost = 1;
                break;
            case 7:
                properties.name = "Snow";
                properties.yields["science"] = 0;
                properties.yields["culture"] = 0;
                properties.yields["gold"] = 0;
                properties.yields["food"] = 0;
                properties.yields["production"] = 0;
                properties.movementCost = 1;
                break;
        }
        return properties;
    }
    //use perlin noise to create smooth transition between hex generation making sure cold tiles are set closer to the poles
    public int GenerateTerrain(int x, int y)
    {
        float noiseValue = Mathf.PerlinNoise(x * scale + seed, y * scale + seed);
        int terrainType = 0;
        bool closeToPole = false;
        if (y < 2 || y > height - 3)
        {
            closeToPole = true;
        }
        else
        {
            closeToPole = false;
        }

        if (!closeToPole)
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
            if (noiseValue < 0.33f)
            {
                terrainType = 6;
            }
            else if (noiseValue < 0.66f)
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
    //sets properties of hexes without perlin noise to avoid unfair terrain
    public (bool hasOasis, bool hasWoods, bool hasHills) AssignTerrainType(string terrainName)
    {
        bool hasOasis = false;
        bool hasWoods = false;
        bool hasHills = false;
        float rand1 = Random.value;
        float rand2 = Random.value;
        switch (terrainName)
        {
            case "Plains":
            case "Grassland":
            case "Tundra":
                if (rand1 < 0.5f)
                {
                    hasWoods = true;
                    if (rand2 < 0.5f)
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
                    if (rand2 < 0.5f)
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
                if (rand1 < 0.5f)
                {
                    hasHills = true;
                }
                else
                {
                    hasHills = false;
                    if (rand2 < 0.5f)
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
                if (rand1 < 0.5f)
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
    //adds points to score depending on current game situation and only to hexes that are part of a city
    public void NextTurn()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].properties.neighbouringCityCenter)
            {
                currentGold += cells[i].properties.yields["gold"];
                currentCulture += cells[i].properties.yields["culture"];
                currentScience += cells[i].properties.yields["science"];
            }
        }
        currentGoldText.text = "Gold: " + currentGold.ToString();
        currentCultureText.text = "Culture: " + currentCulture.ToString();
        currentScienceText.text = "Science: " + currentScience.ToString();
    }
}