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
    public TextMeshProUGUI newCityProductionText;
    public TextMeshProUGUI newCityFoodText;
    public HexMapEditor gameUI;
    public Image units;
    public Image buildings;
    public Image districts;
    public int cityProduction;
    public int cityFood;
    public List<HexCell> cityOwnedTiles = new List<HexCell>();
    public GameObject purchaseButton;
    void Start()
    {
        cityCenterUI = this.GetComponentInChildren<Canvas>();
        cityCenterUI.gameObject.SetActive(false);
        HexGrid.inMenu = false;
        gameUI = GameObject.Find("Hex Map Editor").GetComponent<HexMapEditor>();
        newCityProductionText = Instantiate(cityProductionText, new Vector3(225.8f, 239, 0), Quaternion.identity);
        newCityFoodText = Instantiate(cityFoodText, new Vector3(379, 239, 0), Quaternion.identity);
        newCityProductionText.transform.SetParent(cityCenterUI.transform, false);
        newCityFoodText.transform.SetParent(cityCenterUI.transform, false);
        units = FindInChildrenIncludingInactive(this.gameObject, "units").GetComponent<Image>();
        buildings = FindInChildrenIncludingInactive(this.gameObject, "buildings").GetComponent<Image>();
        districts = FindInChildrenIncludingInactive(this.gameObject, "districts").GetComponent<Image>();
        buildings.gameObject.SetActive(false);
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
                    gameUI.gameObject.SetActive(false);
                    cityCenterUI.gameObject.SetActive(true);
                    PurchasableTiles();
                    HexGrid.inMenu = true;
                    units.gameObject.SetActive(true);
                    buildings.gameObject.SetActive(false);
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
    }
    //goes through list of city owned tiles and adds to that city's production and food counters
    public void AddYieldsFromCityOwnedTiles()
    {
        foreach (HexCell hex in cityOwnedTiles)
        {
            cityProduction += hex.properties.yields["Production"];
            cityFood += hex.properties.yields["Food"];
            newCityProductionText.text = "Production: " + cityProduction;
            newCityFoodText.text = "Food: " + cityFood;
        }
    }
    //creates a button with text showing how much a tile costs for a city to buy depending on distance from city center
    public void PurchasableTiles()
    {
        foreach(HexCell hex in HexGrid.cells)
        {
            switch(HexCoordinates.Heuristic(hex, this.GetComponentInParent<HexCell>()))
            {
                case 2:
                {
                    hex.properties.cost = 50;
                    GameObject newPurchaseButton = Instantiate(purchaseButton, new Vector3(hex.transform.position.x, hex.transform.position.y + 5, hex.transform.position.z), Quaternion.identity);
                    newPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = hex.properties.cost.ToString();
                    newPurchaseButton.transform.SetParent(hex.gameObject.transform);
                }
                break;
                case 3:
                {
                    hex.properties.cost = 75;
                    foreach(BoxCollider coll in hex.properties.colliders)
                    {
                        ColliderScript script = coll.GetComponent<ColliderScript>();
                        if(script.neighbour.properties.ownedByCity)
                        {
                            GameObject newPurchaseButton = Instantiate(purchaseButton, new Vector3(hex.transform.position.x, hex.transform.position.y + 5, hex.transform.position.z), Quaternion.identity);
                            newPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = hex.properties.cost.ToString();
                            newPurchaseButton.transform.SetParent(hex.gameObject.transform);
                        }
                    }
                }
                break;
                default:
                {
                    hex.properties.cost = 2147483647;
                }
                break;
            }
            if(hex.properties.cost <= HexGrid.currentGold)
            {
                hex.properties.purchasable = true;
            }
            else
            {
                hex.properties.purchasable = false;
            }
        }
    }
    public void PurchaseTile()
    {
        Debug.Log("Entered function"); 
        GameObject parentObj = this.transform.root.gameObject;
        HexCell[] hexes = parentObj.GetComponentsInChildren<HexCell>();
        foreach(HexCell hex in hexes)
        {
            Debug.Log(hex.properties.name);
        }
    }
    //activates and deactivates different tabs of city center UI
    public void Units()
    {
        units.gameObject.SetActive(true);
        buildings.gameObject.SetActive(false);
        districts.gameObject.SetActive(false);
    }
    public void Districts()
    {
        units.gameObject.SetActive(false);
        buildings.gameObject.SetActive(true);
        districts.gameObject.SetActive(false);
    }
    public void Buildings()
    {
        units.gameObject.SetActive(false);
        buildings.gameObject.SetActive(false);
        districts.gameObject.SetActive(true);
    }
    //closes city center UI and opens regular UI
    public void Close()
    {
        gameUI.gameObject.SetActive(true);
        cityCenterUI.gameObject.SetActive(false);
        HexGrid.inMenu = false;
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
    /*public static GameObject FindInParent(GameObject gO, string name)
    {
        for(int i = 0; i < gO.transform.hierarchyCount; i++)
        {
            if(gO.transform.parent(i).gameObject.name == name)
            {
                return gO.transform.parent(i).gameObject;
            }
            GameObject found = FindInParent(gO.transform.parent(i).gameObject, name);
            if (found != null)
            {
                return found;
            }
        }
        return null;  //didn't find any
    }*/
}
