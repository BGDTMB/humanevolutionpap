using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WarriorScript : MonoBehaviour
{
    public int maxMP;
    public int currentMP;
    public int maxHP;
    public int currentHP;
    public int dp;
    public int id;
    public GameObject warriorUI;
    public HexGrid hexGrid;
    public HexCell current;
    public GameObject attackBtn;
    public List<GameObject> btns = new List<GameObject>();
    public TextMeshProUGUI hpText;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<HexCell>() != null)
        {
            current = collision.gameObject.GetComponent<HexCell>();
        }
    }
    public void Start()
    {
        currentHP = maxHP;
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
                foreach (GameObject btn in btns)
                {
                    Destroy(btn);
                }
                btns.Clear();
            }
        }
    }
    public void checkAttackable()
    {
        foreach (BoxCollider coll in current.properties.colliders)
        {
            HexCell neighbour = coll.GetComponent<ColliderScript>().neighbour;
            if (neighbour.properties.enemy != null)
            {
                GameObject newBtn = Instantiate(attackBtn, new Vector3(neighbour.transform.position.x, neighbour.transform.position.y + 7, neighbour.transform.position.z), Quaternion.identity);
                newBtn.transform.SetParent(this.transform);
                newBtn.transform.SetParent(neighbour.transform);
                btns.Add(newBtn);
                foreach (GameObject btn in FindObjectsOfType<GameObject>().Where(obj => obj.GetComponentInChildren<MoveToTarget>() != null && obj.scene == SceneManager.GetActiveScene()).ToArray())
                {
                    btn.transform.parent = null;
                    Destroy(btn);
                }
            }
        }
    }
    public void Attack(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.currentHP -= dp;
        currentHP -= enemyScript.dp;
        hpText.text = currentHP + "/" + maxHP;
        enemyScript.hpText.text = enemyScript.currentHP + "/" + enemyScript.maxHP;
        if (enemyScript.currentHP <= 0)
        {
            Destroy(enemy);
        }
        if (currentHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
