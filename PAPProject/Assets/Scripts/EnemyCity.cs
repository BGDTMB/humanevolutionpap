using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCity : MonoBehaviour
{
    public int currentHP;
    public int maxHP;
    public HexGrid hexGrid;
    void Start()
    {
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
    }
    public void Lost()
    {
        HexCell hex = new HexCell();
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponentInParent<HexCell>() != null)
            {
               hex = hitCollider.gameObject.GetComponentInParent<HexCell>();
            }
        }
        hex.properties.hasStructure = false;
        Destroy(this.gameObject);
    }
    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if(currentHP <= 0)
        {
            HexGrid.currentGold += 500;
            hexGrid.currentGoldText.text = "Gold: " + HexGrid.currentGold.ToString();
            Lost();
        }
    }
}
