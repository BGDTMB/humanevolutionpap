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
    public CityCenter ccScrpt;
    void Awake()
    {
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        inConstructionModel = this.gameObject;
    }
    void Start()
    {
        hex = this.GetComponentInParent<HexCell>();
        foreach(BoxCollider coll in hex.properties.colliders)
        {
            if(hexGrid.cities.Contains(coll.GetComponent<ColliderScript>().neighbour))
            {
                ccScrpt = coll.GetComponent<ColliderScript>().neighbour.GetComponentInChildren<CityCenter>();
            }
        }
        if (ccScrpt.cellInConstruction != null)
        {
            ccScrpt.cellInConstruction.properties.hasStructure = false;
            Destroy(ccScrpt.cellInConstruction.GetComponentInChildren<InConstruction>().gameObject);
        }
        ccScrpt.cellInConstruction = hex;
        turnsLeftText = this.GetComponentInChildren<TextMeshPro>();
        cityProduction = ccScrpt.cityProduction;
        switch (buildingName)
        {
            case "Theatre Square":
                finishedModel = Instantiate(hexGrid.theatreSquareModel, this.transform.position, Quaternion.AngleAxis(180, Vector3.up));
                finishedModel.SetActive(false);
                culture = 3;
                break;
            case "Campus":
                finishedModel = Instantiate(hexGrid.campusModel, this.transform.position, Quaternion.identity);
                finishedModel.SetActive(false);
                science = 3;
                break;
            case "Commercial Hub":
                finishedModel = Instantiate(hexGrid.commercialHubModel, this.transform.position, Quaternion.identity);
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
            Destroy(inConstructionModel.gameObject);
            finishedModel.SetActive(true);
            hex.properties.yields["Culture"] += culture;
            hex.properties.yields["Science"] += science;
            hex.properties.yields["Gold"] += gold;
            hexGrid.ResetYields(HexCoordinates.FromCoordinates(HexCoordinates.OffsetCoordinates(hex.coordinates.X, hex.coordinates.Y), hexGrid.width));
            ccScrpt.cellInConstruction = null;
        }
    }
}
