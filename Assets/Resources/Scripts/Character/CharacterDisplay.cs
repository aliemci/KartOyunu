using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class CharacterDisplay : MonoBehaviour
{
    public Character character;
    public bool isPlayer = false;
    public playerCharacter playerChar;
    public bool isEnemy = false;
    public rivalCharacter rivalChar;

    //Her ekran için ayrı büyüklükte olacağı için belirli bir katsayı ile nesneleri büyültüp/küçülteceğiz.
    private Vector2 resolution_scale;
    

    void Awake()
    {
        //Çözünürlük için
        resolution_scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;
    }

    void Start()
    {
        if (character as playerCharacter == null)
            isEnemy = true;
        else
            isPlayer = true;

        //Karakterin çiziminin ekranda çıkması için.
        GetComponent<SpriteRenderer>().sprite = character.CharacterSprite;

        healthWriter();
        manaWriter();

        //Ekrana göre büyüklükleri ayarlanıyor.
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
        
    }


    public void healthWriter()
    {
        if (isPlayer)
        {
            GameObject.Find("Heart_Value").GetComponent<TextMeshProUGUI>().text = character.health.ToString(); // + "/" + character.maxHealth.ToString();
            return;
        }

        //Can yazısı yenileniyor.
        transform.Find("health").GetComponent<TextMeshProUGUI>().text = character.health.ToString(); // + "/" + character.maxHealth.ToString();
    }

    public void manaWriter()
    {
        if (isPlayer)
        {
            GameObject.Find("Mana_Value").GetComponent<TextMeshProUGUI>().text = character.mana.ToString();
        }
    }



    public void checkIsDead()
    {
        healthWriter();
        manaWriter();

        //Ölüm durumu kontrol ediliyor.
        if (character.health <= 0)
        {
            Destroy(gameObject);
        }
    }


}

