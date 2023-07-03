using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int dp;
    public TextMeshProUGUI hpText;

    void Start()
    {
        currentHP = maxHP;
    }
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<HexCell>() != null)
        {
            collision.gameObject.GetComponent<HexCell>().properties.enemy = this.gameObject;
        }
    }
}
