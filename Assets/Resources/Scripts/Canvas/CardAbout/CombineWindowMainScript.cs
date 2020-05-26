using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineWindowMainScript : MonoBehaviour
{
    private GameObject card1, card2;

    private Transform deck;

    private bool isLeftCardSlotEmpty = true, isRightCardSlotEmpty = true;

    private CombineType comTypeCard1, comTypeCard2;

    //-------------------------------------


    private void Start()
    {
        deck = GameObject.Find("Deck").transform;
        Debug.Log(deck + " is asssigned");
    }


    public void CombineThem()
    {
        //Kart kontrolü
        checksCardSlots();

        //Sağ ve solda kartlar varsa
        if (!isLeftCardSlotEmpty && !isRightCardSlotEmpty)
        {
            //Kart tiplerini alıyor.
            CombineType comTypeCard1 = card1.GetComponent<CombineCard>().cardCombineType;
            CombineType comTypeCard2 = card2.GetComponent<CombineCard>().cardCombineType;

            //Yeni kartın özellikleri eski iki kartların toplamı kadar oluyor.
            int cardAttack = card1.GetComponent<CardDisplay>().card.attack + card2.GetComponent<CardDisplay>().card.attack;
            int cardDefence = card1.GetComponent<CardDisplay>().card.defence + card2.GetComponent<CardDisplay>().card.defence;
            int cardMana = card1.GetComponent<CardDisplay>().card.mana + card2.GetComponent<CardDisplay>().card.mana;



        }
        else
        {

            return;
        }

        

    }
    

    private void checksCardSlots()
    {
        if (this.transform.Find("CardSlotLeft").transform.childCount > 0)
            isLeftCardSlotEmpty = false;
        else
            isLeftCardSlotEmpty = true;

        if (this.transform.Find("CardSlotRight").transform.childCount > 0)
            isRightCardSlotEmpty = false;
        else
            isRightCardSlotEmpty = true;
    }


    private void returnCardsToDeck()
    {
        //Kartlar kontrol ediliyor.
        checksCardSlots();

        //Eğer sol kart yeri doluysa
        if (!isLeftCardSlotEmpty)
        {
            card1 = this.transform.Find("CardSlotLeft").transform.GetChild(0).gameObject;
            card1.transform.SetParent(deck);
        }

        //Eğer sağ kart yeri doluysa
        if (!isRightCardSlotEmpty)
        {
            card2 = this.transform.Find("CardSlotRight").transform.GetChild(0).gameObject;
            card2.transform.SetParent(deck);
        }

    }


    public void windowClose()
    {
        //Eğer kart yüklendiyse onları geri yerine koyuyor.
        returnCardsToDeck();

        //Kartlardaki birleşme tuşunu çalışır vaziyete getiriyor.
        foreach(GameObject cardObj in GameObject.FindGameObjectsWithTag("Card"))
        {
            if(cardObj.GetComponent<CombineCard>()){
                cardObj.transform.Find("Combine").gameObject.SetActive(true);
            }
        }

        //Pencereyi kapatıyor.
        //Debug.Log(this.transform.ToString() + " Should be Closed!");
        Destroy(this.transform.gameObject);
    }


}
