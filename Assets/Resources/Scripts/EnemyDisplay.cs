using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;

    public string Id, Description;
    public int Health, Mana, Shield, maxHealth;
    
    void Start()
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
        Health -= damage;
        
        
        //Düşmanın can barına erişip, x ekseninde ölçeklendirme yapıyor. (Çalışma prensibi ile alakalı)
        transform.GetChild(0).GetChild(0).transform.localScale = new Vector3((float)Health / maxHealth, 1f, 1f);

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
