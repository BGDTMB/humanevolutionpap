using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityCenter : MonoBehaviour
{
    public Canvas cityCenterUI;
    public TextMeshProUGUI cityProductionText;
    public TextMeshProUGUI cityFoodText;
    public GameObject gameUI;
    public Image units;
    public Image buildings;
    public Image districts;
    public int cityProduction;
    public int cityFood;
    public List<HexCell> cityOwnedTiles = new List<HexCell>();
    public GameObject purchaseButton;
    public GameObject currentCC;
    public HexGrid hexGrid;
    public GameObject settlerUnit;
    public GameObject warriorUnit;
    public bool buildingSettler = false;
    public bool buildingWarrior = false;
    int turnsToSettler;
    int turnsToWarrior;
    public GameObject turnsToProduceSettlerText;
    public GameObject turnsToProduceWarriorText;
    public HexCell cellInConstruction;
    void Awake()
    {
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        cityProductionText = FindInChildrenIncludingInactive(this.gameObject, "Production").GetComponent<TextMeshProUGUI>();
        cityFoodText = FindInChildrenIncludingInactive(this.gameObject, "Food").GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        hexGrid.cities.Add(this.GetComponentInParent<HexCell>());
        cityCenterUI = this.GetComponentInChildren<Canvas>();
        cityCenterUI.gameObject.SetActive(false);
        HexGrid.inMenu = false;
        gameUI = GameObject.Find("MainUI");
        units = FindInChildrenIncludingInactive(this.gameObject, "units").GetComponent<Image>();
        districts = FindInChildrenIncludingInactive(this.gameObject, "districts").GetComponent<Image>();
        districts.gameObject.SetActive(false);
        CheckDistanceFromCityCenter();
        AddYieldsFromCityOwnedTiles();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                //show city center UI and hides regular UI when city center selected
                if (hit.collider.gameObject.name == this.gameObject.name)
                {
                    gameUI.SetActive(false);
                    cityCenterUI.gameObject.SetActive(true);
                    HexGrid.inMenu = true;
                    units.gameObject.SetActive(true);
                    districts.gameObject.SetActive(false);
                }
            }
        }
    }
    //use heuristic function to find hexes that are one 'step' away from city center to consider as tiles owned by city
    public void CheckDistanceFromCityCenter()
    {
        foreach (HexCell hex in HexGrid.cells)
        {
            if (HexCoordinates.Heuristic(hex, this.GetComponentInParent<HexCell>()) < 2)
            {
                hex.properties.ownedByCity = true;
                hex.transform.SetParent(this.transform, true);
                cityOwnedTiles.Add(hex);
            }
        }
        hexGrid.cellsOwnedByCity.AddRange(cityOwnedTiles);
    }
    //goes through list of city owned tiles and adds to that city's production and food counters
    public void AddYieldsFromCityOwnedTiles()
    {
        foreach (HexCell hex in cityOwnedTiles)
        {
            cityProduction += hex.properties.yields["Production"];
            cityFood += hex.properties.yields["Food"];
            cityProductionText.text = "Production: " + cityProduction;
            cityFoodText.text = "Food: " + cityFood;
        }
    }
    
    //activates and deactivates different tabs of city center UI
    public void Units()
    {
        units.gameObject.SetActive(true);
        districts.gameObject.SetActive(false);
    }
    public void Buildings()
    {
        units.gameObject.SetActive(false);
        districts.gameObject.SetActive(true);
    }
    //closes city center UI and opens regular UI
    public void Close()
    {
        gameUI.gameObject.SetActive(true);
        cityCenterUI.gameObject.SetActive(false);
        HexGrid.inMenu = false;
    }
    public void TurnsToSettler()
    {
        int necessaryProd = 30 * hexGrid.settlersTrained + 50;
        turnsToSettler = necessaryProd / cityProduction;
        turnsToProduceSettlerText.SetActive(true);
        turnsToProduceSettlerText.GetComponent<TextMeshProUGUI>().text = "Turns Left: " + turnsToSettler.ToString();
        buildingSettler = true;
    }
    public void SettlerTurnCounter()
    {
        if(turnsToSettler <= 1)
        {
            CreateSettler();
            turnsToProduceSettlerText.SetActive(false);
            buildingSettler = false;
        }
        else
        {
            turnsToSettler -= 1;
            turnsToProduceSettlerText.GetComponent<TextMeshProUGUI>().text = "Turns Left: " + turnsToSettler.ToString();
        }
    }
    public void CreateSettler()
    {
        hexGrid.settlersTrained += 1;
        Instantiate(settlerUnit, this.transform.position, Quaternion.AngleAxis(90, Vector3.left));
    }
    public void TurnsToWarrior()
    {
        int necessaryProd = 10 * hexGrid.warriorsTrained + 30;
        turnsToWarrior = necessaryProd / cityProduction;
        turnsToProduceWarriorText.SetActive(true);
        turnsToProduceWarriorText.GetComponent<TextMeshProUGUI>().text = "Turns Left: " + turnsToWarrior.ToString();
        buildingWarrior = true;
    }
    public void WarriorTurnCounter()
    {
        if(turnsToWarrior <= 1)
        {
            CreateWarrior();
            turnsToProduceWarriorText.SetActive(false);
            buildingWarrior = false;
        }
        else
        {
            turnsToWarrior -= 1;
            turnsToProduceWarriorText.GetComponent<TextMeshProUGUI>().text = "Turns Left: " + turnsToWarrior.ToString();
        }
    }
    public void CreateWarrior()
    {
        hexGrid.warriorsTrained += 1;
        Instantiate(warriorUnit, this.transform.position, Quaternion.AngleAxis(90, Vector3.left));
    }
    //goes through children of an object and finds child through name even if that child is inactive as the .Find() function cannot do the same
    public static GameObject FindInChildrenIncludingInactive(GameObject gO, string name)
    {

        for (int i = 0; i < gO.transform.childCount; i++)
        {
            if (gO.transform.GetChild(i).gameObject.name == name)
            {
                return gO.transform.GetChild(i).gameObject;
            }

            GameObject found = FindInChildrenIncludingInactive(gO.transform.GetChild(i).gameObject, name);
            if (found != null)
            {
                return found;
            }
        }
        return null;  //didn't find any
    }
}
