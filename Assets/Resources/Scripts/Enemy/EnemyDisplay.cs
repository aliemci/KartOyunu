using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;

    public string Id, Description;
    public int Health, Mana, Shield, maxHealth;

    private Vector2 resolution_scale;

    void Awake()
    {
        resolution_scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;
        //Debug.Log(resolution_scale);
    }

    void Start()
    {
        Refresh();
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Health.ToString() + "/" + maxHealth.ToString();
    }

    public void Refresh()
    {
        Id = enemy.Id;
        Description = enemy.Description;
        Health = enemy.Health;
        maxHealth = Health;
        Mana = enemy.Mana;
        Shield = enemy.Shield;
        GetComponent<SpriteRenderer>().sprite = enemy.Character;
    }

    public void DamageTaken(int damage)
    {
        //Helath update
        Health -= damage;   
        
        //Health text update
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Health.ToString() + "/" + maxHealth.ToString();

        //Die condition
        if (Health <= 0)
        {
            Death();
        }
    }

    void Death()
    { 
        Destroy(gameObject);
        //Debug.Log("Enemy is Dead!");
    }
    
}
