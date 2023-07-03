using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int dp;

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<HexCell>() != null)
        {
            collision.gameObject.GetComponent<HexCell>().properties.enemy = this.gameObject;
        }
    }
}
