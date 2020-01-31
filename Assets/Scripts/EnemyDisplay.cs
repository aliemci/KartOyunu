using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;

    public string Id, Description;
    public int Health, Mana, Shield;
    
    void Start()
    {
        Id = enemy.Id;
        Description = enemy.Description;
        Health = enemy.Health;
        Mana = enemy.Mana;
        Shield = enemy.Shield;
        GetComponent<SpriteRenderer>().sprite = enemy.Character;
    }

    public void DamageTaken(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
        Debug.Log("Düşman Öldü!");
    }
    
}
