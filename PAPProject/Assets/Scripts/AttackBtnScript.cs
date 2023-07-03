using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBtnScript : MonoBehaviour
{
    public int attackType = 0; //1 - unit; 2 - city
    public void Attack()
    {
        if(attackType == 1)
        {
            AttackUnit();
        }
        else if(attackType == 2)
        {
            AttackCity();
        }
    }
    public void AttackUnit()
    {
        this.GetComponentInParent<WarriorScript>().AttackUnit(this.gameObject.GetComponentInParent<HexCell>().properties.enemy);
    }
    public void AttackCity()
    {
        this.GetComponentInParent<WarriorScript>().AttackCity(this.gameObject.GetComponentInParent<HexCell>());
    }
}
