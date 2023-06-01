using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InConstruction : MonoBehaviour
{
    public string buildingName;
    public int cityProduction;
    public int howManyTurnsToFinish;
    public int howManyTurnsPassed;
    public GameObject inConstructionModel;
    public GameObject finishedModel;
    public int culture = 0;
    public int science = 0;
    public int gold = 0;
    public HexGrid hexGrid;
    // Start is called before the first frame update
    void Start()
    {
        hexGrid = this.GetComponentInParent<HexGrid>();
        CityCenter ccScrpt = GetComponentInParent<CityCenter>();
        cityProduction = ccScrpt.cityProduction;
        inConstructionModel = this.gameObject;
        switch (buildingName)
        {
            case "Theatre Square":
                finishedModel = Instantiate(ccScrpt.gameObject.GetComponentInParent<HexGrid>().theatreSquareModel, this.transform.position, Quaternion.identity);
                finishedModel.SetActive(false);
                culture = 3;
                break;
            case "Campus":
                finishedModel = Instantiate(ccScrpt.gameObject.GetComponentInParent<HexGrid>().campusModel, this.transform.position, Quaternion.identity);
                finishedModel.SetActive(false);
                science = 3;
                break;
            case "Commercial Hub":
                finishedModel = Instantiate(ccScrpt.gameObject.GetComponentInParent<HexGrid>().commercialHubModel, this.transform.position, Quaternion.identity);
                finishedModel.SetActive(false);
                gold = 3;
                break;
        }
        howManyTurnsToFinish = 60 / cityProduction;
        Debug.Log(howManyTurnsToFinish);
    }

    // Update is called once per frame
    void Update()
    {
        if (howManyTurnsToFinish == 0)
        {
            inConstructionModel.SetActive(false);
            finishedModel.SetActive(true);
            HexCell hex = this.GetComponentInParent<HexCell>();
            hex.properties.yields["Culture"] += culture;
            hex.properties.yields["Science"] += science;
            hex.properties.yields["Gold"] += gold;
            Debug.Log(culture);
            Debug.Log(science);
            Debug.Log(gold);
            Debug.Log(hex.properties.yields["Culture"]);
            Debug.Log(hex.properties.yields["Science"]);
            Debug.Log(hex.properties.yields["Gold"]);
            hexGrid.ShowYields(HexCoordinates.FromCoordinates(HexCoordinates.FromPosition(hex.transform.position), HexGrid.width));
        }
    }
}
