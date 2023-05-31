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
    public GameObject theatreSquareModel;
    public GameObject campusModel;
    public GameObject commercialHubModel;
    // Start is called before the first frame update
    void Start()
    {
        CityCenter ccScrpt = GetComponentInParent<CityCenter>();
        cityProduction = ccScrpt.cityProduction;
        inConstructionModel = this.gameObject;
        switch (buildingName)
        {
            case "Theatre Square":
                finishedModel = theatreSquareModel;
                break;
            case "Campus":
                finishedModel = campusModel;
                break;
            case "Commercial Hub":
                finishedModel = commercialHubModel;
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
            Debug.Log("entered");
            inConstructionModel.SetActive(false);
            finishedModel.SetActive(true);
        }
    }
    public void TurnCounter()
    {
        howManyTurnsToFinish--;
    }
}
