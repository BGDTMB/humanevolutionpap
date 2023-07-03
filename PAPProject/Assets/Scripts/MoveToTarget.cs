using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    private IEnumerator coroutine;
    public HexCell target;
    public List<HexCell> shortestPath = new List<HexCell>();
    public UnitMovement unit;
    public GameObject icon;
    public void MoveTo()
    {
        StartCoroutine(Steps());
    }
    public IEnumerator Steps()
    {
        target = this.GetComponentInParent<HexCell>();
        unit = this.GetComponentInParent<UnitMovement>();
        icon = unit.gameObject.transform.Find("WorldSpaceCanvas").gameObject;
        shortestPath = unit.GetShortestPath(target);
        unit.transform.DetachChildren();
        icon.transform.SetParent(unit.transform);
        icon.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y + 9, unit.transform.position.z);
        for (int i = 0; i < shortestPath.Count; i++)
        {
            HexCell hex = shortestPath[i];
            unit.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y + 5, hex.transform.position.z);
            if (unit.gameObject.name == "Settler" || unit.gameObject.name == "Settler(Clone)")
            {
                unit.gameObject.GetComponent<SettlerScript>().currentMP -= unit.visited[hex];
            }
            else if (unit.gameObject.name == "Warrior" || unit.gameObject.name == "Warrior(Clone)")
            {
                unit.gameObject.GetComponent<WarriorScript>().currentMP -= unit.visited[hex];
            }
            if (i == shortestPath.Count - 1)
            {
                unit.visited = new Dictionary<HexCell, int>();
                unit.unvisited = new Dictionary<HexCell, int>();
                unit.pathToEachHex = new Dictionary<HexCell, List<HexCell>>();
                unit.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y + 5, hex.transform.position.z);
                unit.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y - 1, hex.transform.position.z);
            }
            yield return new WaitForSeconds(0.4f);
        }
        foreach (GameObject btn in unit.btns)
        {
            Destroy(btn);
        }
    }
}
