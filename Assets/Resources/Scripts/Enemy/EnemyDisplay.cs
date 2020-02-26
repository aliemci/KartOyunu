using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;
    string Id, Description;
    int Health, Mana, Shield, maxHealth;
    

    //Her ekran için ayrı büyüklükte olacağı için
    private Vector2 resolution_scale;


    void Awake()
    {
        resolution_scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;
    }


    void Start()
    {
        //Düşman bilgileri ekleniyor.
        Refresh();
        //Ekrana göre büyüklükleri ayarlanıyor.
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
        //Can yazısı yazdırılıyor.
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Health.ToString() + "/" + maxHealth.ToString();
    }

    public void Refresh()
    {
        //Düşman bilgileri ataması
        Id = enemy.Id;
        Description = enemy.Description;
        Health = enemy.Health;
        maxHealth = Health;
        Mana = enemy.Mana;
        Shield = enemy.Shield;
        GetComponent<SpriteRenderer>().sprite = enemy.CharacterSprite;
    }

    public void DamageTaken(int damage)
    {
        //Can aldığı hasar kadar azalıyor.
        Health -= damage;   
        
        //Can yazısı yenileniyor.
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Health.ToString() + "/" + maxHealth.ToString();

        //Ölüm durumu kontrol ediliyor.
        if (Health <= 0)
        {
            Death();
        }
    }

    void Death()
    { 
        //Ölüm durumunda nesne yok ediliyor.
        Destroy(gameObject);
    }
    
    public void EnemyMoves()
    {

    }
}
