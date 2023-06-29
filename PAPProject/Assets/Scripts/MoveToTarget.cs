using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    private IEnumerator coroutine;
    public HexCell target;
    public List<HexCell> shortestPath = new List<HexCell>();
    public UnitMovement unit;
    public void MoveTo()
    {
        StartCoroutine(Steps());
    }
    public IEnumerator Steps()
    {
        target = this.GetComponentInParent<HexCell>();
        unit = this.GetComponentInParent<UnitMovement>();
        shortestPath = unit.GetShortestPath(target);
        unit.transform.DetachChildren();
        for (int i = 0; i < shortestPath.Count; i++)
        {
            HexCell hex = shortestPath[i];
            unit.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y + 5, hex.transform.position.z);
            if (i == shortestPath.Count - 1)
            {
                unit.currentMP -= unit.visited[hex];
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
