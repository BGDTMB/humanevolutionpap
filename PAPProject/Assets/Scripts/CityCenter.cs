using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCenter : MonoBehaviour
{
    public Canvas cityCenterUI;
    public GameObject newCityCenterUI;
    public HexMapEditor gameUI;
    public GameObject testOne;
    public GameObject testTwo;
    public GameObject testThree;
    void Start()
    {
        cityCenterUI = this.GetComponentInChildren<Canvas>();
        cityCenterUI.gameObject.SetActive(false);
        HexGrid.inMenu = false;
        gameUI = GameObject.Find("Hex Map Editor").GetComponent<HexMapEditor>();
        CheckDistanceFromCityCenter();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		    RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
			{
				if(hit.collider.gameObject.name == this.gameObject.name)
                {
                    gameUI.gameObject.SetActive(false);
                    cityCenterUI.gameObject.SetActive(true);
                    HexGrid.inMenu = true;
                }
                else
                {
                    gameUI.gameObject.SetActive(true);
                    cityCenterUI.gameObject.SetActive(false);
                    HexGrid.inMenu = false;
                }
			}
        }
    }
    public void CheckDistanceFromCityCenter()
    {
        foreach(HexCell hex in HexGrid.cells)
        {
            if(HexCoordinates.Heuristic(hex, this.GetComponentInParent<HexCell>()) == 1)
            {
                hex.properties.neighbouringCityCenter = true;
                hex.transform.SetParent(this.transform, true);
            }
        }
    }
    public void Units()
    {
        
    }
    public void Districts()
    {
        
    }
    public void Buildings()
    {
        
    }
}
