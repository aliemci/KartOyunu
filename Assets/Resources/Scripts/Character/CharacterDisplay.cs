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
    public bool isEnemy = false;

    private end_turn end_turn_ref;

    //Her ekran için ayrı büyüklükte olacağı için belirli bir katsayı ile nesneleri büyültüp/küçülteceğiz.
    private Vector2 resolution_scale;

    public TextMeshProUGUI hearthText, manaText;

    void Awake()
    {
        //Çözünürlük için
        resolution_scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;

        end_turn_ref = GameObject.Find("Next_Turn").GetComponent<end_turn>();
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
        situationUpdater();
    }


    public void situationUpdater()
    {
        //Eğer oyuncuysa
        if (isPlayer)
        {
            //Can ve Enerji değerlerini yazdırıyor.
            hearthText.text = character.health.ToString();
            manaText.text = character.mana.ToString();
            //Eğer ölmüşse
            if (character.health <= 0)
            {
                //Can yerine 0 yazdırılacak
                hearthText.text = "0";
                //Nesne silinecek.
                Destroy(this.gameObject);
            }
        }
        //Eğer oyuncu değilse
        else
        {
            Debug.Log(character.name + "'s situation updater!");
            //Can değerlerini yazdırıyor.
            transform.Find("health").GetComponent<TextMeshProUGUI>().text = character.health.ToString();
            //Eğer ölmüşse
            if (character.health <= 0)
            {
                Debug.Log(character.name + " is dead!");
                //Bir düşman öldüğüne göre düşman listesinin güncellenmesi gerekmekte. Bu işe yarıyor.
                end_turn_ref.isVariablesDefined = false;

                //Dövüşün bitip bitmediğini kontrol ediyor.
                end_turn_ref.end_of_fight();

                //Nesneyi siliyor.
                Destroy(this.gameObject);

            }
        }

    }

    /*
    public void healthManaWriter()
    {

        if (isPlayer)
        {
            checkIsDead();
            hearthText.text = character.health.ToString();
            manaText.text = character.mana.ToString();
            return;
        }

        //Can yazısı yenileniyor.
        transform.Find("health").GetComponent<TextMeshProUGUI>().text = character.health.ToString();
    }
    
    public void checkIsDead()
    {
        //Eğer oyuncu değilse
        if(!isPlayer)
            healthManaWriter();

        //Ölüm durumu kontrol ediliyor.
        if (character.health <= 0)
        {
            //Can yazısını sıfır yapıyor.
            hearthText.text = "0";

            Debug.Log("");

            //Oyuncu nesnesini siliyor.
            Destroy(gameObject);
        }
    }
    */

    public void cardRequirements(Character player)
    {
        foreach(GameObject card in GameObject.FindGameObjectsWithTag("Card"))
        {
            //Debug.Log(card.GetComponent<CardDisplay>().card.name + " is checking...");
            try
            {
                int cardMana = card.GetComponentInChildren<CardDisplay>().card.mana;
                //Debug.Log("player:" + player.mana + " ? " + cardMana + " Card");
                if (player.mana < Mathf.Abs(cardMana))
                {
                    card.GetComponent<CardDisplay>().toggleCard(false);
                    //Debug.Log("Card has toggled off");
                }
                else
                {
                    card.GetComponent<CardDisplay>().toggleCard(true);
                    //Debug.Log("Card has toggled on");
                }
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }


}

