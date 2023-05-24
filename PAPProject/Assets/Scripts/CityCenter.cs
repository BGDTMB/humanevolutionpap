using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCenter : MonoBehaviour
{
    public Canvas cityCenterUI;
    public GameObject newCityCenterUI;
    public HexMapEditor gameUI;
    public Image testOne;
    public Image testTwo;
    public Image testThree;
    void Start()
    {
        cityCenterUI = this.GetComponentInChildren<Canvas>();
        cityCenterUI.gameObject.SetActive(false);
        HexGrid.inMenu = false;
        gameUI = GameObject.Find("Hex Map Editor").GetComponent<HexMapEditor>();
        testOne = FindInChildrenIncludingInactive(this.gameObject, "test1").GetComponent<Image>();
        testTwo = FindInChildrenIncludingInactive(this.gameObject, "test2").GetComponent<Image>();
        testThree = FindInChildrenIncludingInactive(this.gameObject, "test3").GetComponent<Image>();
        CheckDistanceFromCityCenter();
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
                    HexGrid.inMenu = true;
                    testOne.gameObject.SetActive(true);
                    testTwo.gameObject.SetActive(false);
                    testThree.gameObject.SetActive(false);
                }
            }
        }
    }
    //use heuristic function to find hexes that are one 'step' away from city center to consider as neighbours
    public void CheckDistanceFromCityCenter()
    {
        foreach (HexCell hex in HexGrid.cells)
        {
            if (HexCoordinates.Heuristic(hex, this.GetComponentInParent<HexCell>()) == 1)
            {
                hex.properties.neighbouringCityCenter = true;
                hex.transform.SetParent(this.transform, true);
            }
        }
    }
    //different tabs of city center UI
    public void Units()
    {
        testOne.gameObject.SetActive(true);
        testTwo.gameObject.SetActive(false);
        testThree.gameObject.SetActive(false);
    }
    public void Districts()
    {
        testOne.gameObject.SetActive(false);
        testTwo.gameObject.SetActive(true);
        testThree.gameObject.SetActive(false);
    }
    public void Buildings()
    {
        testOne.gameObject.SetActive(false);
        testTwo.gameObject.SetActive(false);
        testThree.gameObject.SetActive(true);
    }
    //closes city center UI and opens regular UI
    public void Close()
    {
        gameUI.gameObject.SetActive(true);
        cityCenterUI.gameObject.SetActive(false);
        HexGrid.inMenu = false;
    }
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

        return null;  //didn't find
    }
}
