using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBtnScript : MonoBehaviour
{
    HexGrid hexGrid;
    void Start()
    {
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
    }
    public void Attack()
    {
        this.GetComponentInParent<WarriorScript>().Attack(HexGrid.cells[HexCoordinates.FromCoordinates(HexCoordinates.FromPosition(new Vector3(this.transform.position.x, this.transform.position.y - 7, this.transform.position.z)), hexGrid.width)].properties.enemy);
    }
}
