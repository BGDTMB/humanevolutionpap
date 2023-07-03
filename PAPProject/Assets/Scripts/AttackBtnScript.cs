using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBtnScript : MonoBehaviour
{
    public void Attack()
    {
        this.GetComponentInParent<WarriorScript>().Attack(this.gameObject.GetComponentInParent<HexCell>().properties.enemy);
    }
}
