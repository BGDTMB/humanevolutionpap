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
                    StartCoroutine(checkAttackable());
                }
            }
        }
    }
    public IEnumerator checkAttackable()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (BoxCollider coll in current.properties.colliders)
        {
            HexCell neighbour = coll.GetComponent<ColliderScript>().neighbour;
            if(neighbour != null)
            {
                if (neighbour.properties.enemy != null)
                {
                    GameObject newBtn = Instantiate(attackBtn, new Vector3(neighbour.transform.position.x, neighbour.transform.position.y + 7, neighbour.transform.position.z), Quaternion.identity);
                    newBtn.transform.SetParent(this.transform);
                    newBtn.transform.SetParent(neighbour.transform);
                    newBtn.GetComponent<AttackBtnScript>().attackType = 1;
                    btns.Add(newBtn);
                    Collider[] hitColliders = Physics.OverlapSphere(new Vector3(neighbour.transform.position.x, neighbour.transform.position.y + 7, neighbour.transform.position.z), 0.01f);
                    foreach (var hitCollider in hitColliders)
                    {
                        if (hitCollider.gameObject.name == "MoveTo(Clone)")
                        {
                            Destroy(hitCollider.gameObject);
                        }
                    }
                }
                Collider[] hitCollidersTwo = Physics.OverlapSphere(neighbour.transform.position, 1f);
                foreach (var hitColliderTwo in hitCollidersTwo)
                {
                    if (hitColliderTwo.gameObject.GetComponentInParent<EnemyCity>() != null)
                    {
                        GameObject newBtn = Instantiate(attackBtn, new Vector3(neighbour.transform.position.x, neighbour.transform.position.y + 7, neighbour.transform.position.z), Quaternion.identity);
                        newBtn.transform.SetParent(this.transform);
                        newBtn.transform.SetParent(neighbour.transform);
                        newBtn.GetComponent<AttackBtnScript>().attackType = 2;
                        btns.Add(newBtn);
                        Collider[] hitCollidersThree = Physics.OverlapSphere(new Vector3(neighbour.transform.position.x, neighbour.transform.position.y + 7, neighbour.transform.position.z), 0.01f);
                        foreach (var hitColliderThree in hitCollidersThree)
                        {
                            if (hitColliderThree.gameObject.name == "MoveTo(Clone)")
                            {
                                Destroy(hitColliderThree.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
    public void AttackUnit(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.currentHP -= dp;
        currentHP -= enemyScript.dp;
        hpText.text = currentHP + "/" + maxHP;
        enemyScript.hpText.text = enemyScript.currentHP + "/" + enemyScript.maxHP;
        if (enemyScript.currentHP <= 0)
        {
            Destroy(enemy);
            foreach(GameObject btn in btns)
            {
                Destroy(btn);
            }
        }
        if (currentHP <= 0)
        {
            this.gameObject.transform.parent = null;
            this.gameObject.transform.DetachChildren();
            foreach(GameObject btn in this.gameObject.GetComponent<UnitMovement>().btns)
            {
                Destroy(btn);
            }
            foreach(GameObject btn in btns)
            {
                Destroy(btn);
            }
            hexGrid.warriors.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
    public void AttackCity(HexCell hex)
    {
        GameObject city = new GameObject();
        Collider[] hitColliders = Physics.OverlapSphere(hex.transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponentInParent<EnemyCity>() != null)
            {
                city = hitCollider.gameObject.GetComponentInParent<EnemyCity>().gameObject;
            }
        }
        foreach(GameObject btn in btns)
        {
            Destroy(btn);
        }
        city.GetComponent<EnemyCity>().TakeDamage(dp);
    }
}
