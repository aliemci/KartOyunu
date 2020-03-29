using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;

public class CharacterDisplay : MonoBehaviour
{
    public Character character;
    public bool isPlayer = false;
    //public playerCharacter playerChar;
    public bool isEnemy = false;
    //public rivalCharacter rivalChar;

    //Her ekran için ayrı büyüklükte olacağı için belirli bir katsayı ile nesneleri büyültüp/küçülteceğiz.
    private Vector2 resolution_scale;

    public TextMeshProUGUI hearthText, manaText;

    void Awake()
    {
        //Çözünürlük için
        resolution_scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;
        
    }

    void Start()
    {
        if (character as playerCharacter == null)
        {
            isEnemy = true;
        }
        else
        {
            isPlayer = true;
            cardRequirements(character as playerCharacter);
        }

        //Karakterin çiziminin ekranda çıkması için.
        GetComponent<SpriteRenderer>().sprite = character.CharacterSprite;

        //Ekrana göre büyüklükleri ayarlanıyor.
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
        
        hearthText = GameObject.Find("Heart_Value").GetComponent<TextMeshProUGUI>();
        manaText = GameObject.Find("Mana_Value").GetComponent<TextMeshProUGUI>();
        healthManaWriter();
    }


    public void healthManaWriter()
    {
        //Debug.Log("healthManaWriter is called");
        if (isPlayer)
        {
            //Debug.Log("and it's player");
            hearthText.text = character.health.ToString(); // + "/" + character.maxHealth.ToString();
            manaText.text = character.mana.ToString();
            return;
        }

        //Can yazısı yenileniyor.
        transform.Find("health").GetComponent<TextMeshProUGUI>().text = character.health.ToString(); // + "/" + character.maxHealth.ToString();
    }
    
    public void checkIsDead()
    {
        healthManaWriter();

        //Ölüm durumu kontrol ediliyor.
        if (character.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void cardRequirements(playerCharacter player)
    {
        foreach(GameObject card in GameObject.FindGameObjectsWithTag("Card"))
        {
            Debug.Log(card.GetComponent<CardDisplay>().card.name + " is checking...");
            try
            {
                int cardMana = card.GetComponentInChildren<CardDisplay>().card.mana;
                Debug.Log("player:" + player.mana + " ? " + cardMana + " Card");
                if (player.mana < Mathf.Abs(cardMana))
                {
                    card.GetComponent<CardDisplay>().toggleCard(false);
                    Debug.Log("Card has toggled off");
                }
                else
                {
                    card.GetComponent<CardDisplay>().toggleCard(true);
                    Debug.Log("Card has toggled on");
                }
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}

