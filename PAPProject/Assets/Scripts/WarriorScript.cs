using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorScript : MonoBehaviour
{
    public int maxMP;
    public int currentMP;
    public int hp;
    public int dp;
    public int id;
    public GameObject warriorUI;
    public HexGrid hexGrid;
    public HexCell current;
    public GameObject attackBtn;
    public List<GameObject> btns = new List<GameObject>();
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<HexCell>() != null)
        {
            current = collision.gameObject.GetComponent<HexCell>();
        }
    }
    public void Start()
    {
        currentMP = maxMP;
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        id = hexGrid.warriorsTrained;
        warriorUI = GetComponentInChildren<Canvas>().gameObject;
        warriorUI.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            WhenClicked();
        }
    }
    public void WhenClicked()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            if (hit.transform.GetComponentInChildren<WarriorScript>() != null)
            {
                if (hit.transform.GetComponentInChildren<WarriorScript>().id == id)
                {
                    warriorUI.SetActive(true);
                    checkAttackable();
                }
            }
            else
            {
                warriorUI.SetActive(false);
            }
        }
    }
    public void checkAttackable()
    {
        foreach(BoxCollider coll in current.properties.colliders)
        {
            HexCell neighbour = coll.GetComponent<ColliderScript>().neighbour;
            if(neighbour.properties.enemy != null)
            {
                GameObject newBtn = Instantiate(attackBtn, new Vector3(neighbour.transform.position.x, neighbour.transform.position.y + 7, neighbour.transform.position.z), Quaternion.identity);
                newBtn.transform.SetParent(this.transform);
                btns.Add(newBtn);
            }
        }
    }
    public void Attack(GameObject enemy)
    {
        Debug.Log(enemy);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.hp -= dp;
        hp -= enemyScript.dp;
        Debug.Log(this.gameObject.name + " attacked " + enemy.name);
        if(enemyScript.hp <= 0)
        {
            Destroy(enemy);
        }
        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
