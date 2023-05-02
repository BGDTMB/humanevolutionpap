using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    public HexCell neighbour;
    private void OnCollisionEnter(Collision collision)
    {
        neighbour = collision.transform.parent.gameObject.GetComponent<HexCell>();
    }
}