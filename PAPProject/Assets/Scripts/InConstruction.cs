using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public TextMeshPro turnsLeftText;
    public HexCell hex;
    void Awake()
    {
        inConstructionModel = this.gameObject;
    }
    void Start()
    {
        hexGrid = GetComponentInParent<HexGrid>();
        HexCell hex = this.GetComponentInParent<HexCell>();
        hexGrid.cellsInConstruction.Add(hex);
        turnsLeftText = this.GetComponentInChildren<TextMeshPro>();
        hexGrid = this.GetComponentInParent<HexGrid>();
        CityCenter ccScrpt = GetComponentInParent<CityCenter>();
        cityProduction = ccScrpt.cityProduction;
        switch (buildingName)
        {
            case "Theatre Square":
                finishedModel = Instantiate(ccScrpt.gameObject.GetComponentInParent<HexGrid>().theatreSquareModel, this.transform.position, Quaternion.AngleAxis(180, Vector3.up));
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
        turnsLeftText.text = "Turns Left: " + howManyTurnsToFinish;
    }

    // Update is called once per frame
    void Update()
    {
        if (howManyTurnsToFinish == 0)
        {
            HexCell hex = this.GetComponentInParent<HexCell>();
            inConstructionModel.SetActive(false);
            finishedModel.SetActive(true);
            hex.properties.yields["Culture"] += culture;
            hex.properties.yields["Science"] += science;
            hex.properties.yields["Gold"] += gold;
            hexGrid.ResetYields(HexCoordinates.FromCoordinates(HexCoordinates.OffsetCoordinates(hex.coordinates.X, hex.coordinates.Y), hexGrid.width));
            hexGrid.cellsInConstruction.Remove(hex);
        }
    }
}
