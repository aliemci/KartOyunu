using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombineWindowMainScript : MonoBehaviour
{
    private GameObject card1, card2;

    private Transform deck;

    private bool isLeftCardSlotEmpty = true, isRightCardSlotEmpty = true;

    private CombineType comTypeCard1, comTypeCard2;

    Card thermiteGrenade;
    Card stunGrenade;
    Card chemicalGrenade;
    Card Plasma;
    Card Gase;
    Card Coma;

    //-------------------------------------


    private void Start()
    {
        deck = GameObject.Find("Deck").transform;
        Debug.Log(deck + " is asssigned");

        thermiteGrenade = Resources.Load<Card>("Cards/Thermite Grenade");
        stunGrenade = Resources.Load<Card>("Cards/Stun Grenade");
        chemicalGrenade = Resources.Load<Card>("Cards/Chemical Grenade");

    }


    public void CombineThem()
    {
        //Kart kontrolü
        checksCardSlots();

        //Sağ ve solda kartlar varsa
        if (!isLeftCardSlotEmpty && !isRightCardSlotEmpty)
        {
            //Kart oyun nesneleri atanıyor.
            card1 = transform.Find("CardSlotLeft").GetChild(0).gameObject;
            card2 = transform.Find("CardSlotRight").GetChild(0).gameObject;

            //Kart tiplerini alıyor.
            CombineType comTypeCard1 = card1.GetComponent<CardDisplay>().card.cardCombine;
            CombineType comTypeCard2 = card2.GetComponent<CardDisplay>().card.cardCombine;

            //Yeni kartın özellikleri eski iki kartların toplamı kadar oluyor.
            int cardAttack = card1.GetComponent<CardDisplay>().card.attack + card2.GetComponent<CardDisplay>().card.attack;
            int cardDefence = card1.GetComponent<CardDisplay>().card.defence + card2.GetComponent<CardDisplay>().card.defence;
            int cardMana = card1.GetComponent<CardDisplay>().card.mana + card2.GetComponent<CardDisplay>().card.mana;

            //Kart taslağı
            GameObject CardGO = Resources.Load<GameObject>("Prefabs/Card");
           
            //Basınç + Alev = Thermite Grenade
            if((comTypeCard1 == CombineType.Pressure && comTypeCard2 == CombineType.Flame) || (comTypeCard1 == CombineType.Flame && comTypeCard2 == CombineType.Pressure))
            {
                Debug.Log("THERMITE!!!!");

                //Her yere vuruyor
                float[] cardAttackRange = { 0.25f, 0.25f, 0.25f, 0.25f };

                //Yeni bir kart oluşturuyor.
                Card will_card = ScriptableObject.CreateInstance("Card") as Card;

                //Kartın özelliklerini atıyor
                will_card.cardName = "Thermite";
                will_card.attack = cardAttack;
                will_card.defence = cardDefence;
                will_card.mana = cardMana;
                will_card.attack_range = cardAttackRange;
                will_card.cardDebuff = debuffs.Burn;

                //CardGenerator.create_new_card(CardGO, "Thermite Grenade", thermiteGrenade, transform.Find("CardSlotResult"));
                GameObject createdCard = CardGenerator.create_new_card(CardGO, "Thermite Grenade", will_card, transform.Find("CardSlotResult"));
            }

            //Basınç + Elektrik = Stun Grenade
            if ((comTypeCard1 == CombineType.Pressure && comTypeCard2 == CombineType.Lightning) || (comTypeCard1 == CombineType.Lightning && comTypeCard2 == CombineType.Pressure))
            {
                Debug.Log("STUN!!!!");

                //Her yere vuruyor
                float[] cardAttackRange = { 0.25f, 0.25f, 0.25f, 0.25f };

                //Yeni bir kart oluşturuyor.
                Card will_card = ScriptableObject.CreateInstance("Card") as Card;

                //Kartın özelliklerini atıyor
                will_card.cardName = "Stun Grenade";
                will_card.attack = 0;
                will_card.defence = 0;
                will_card.mana = cardMana;
                will_card.attack_range = cardAttackRange;
                will_card.cardDebuff = debuffs.Stun;

                //CardGenerator.create_new_card(CardGO, "Thermite Grenade", thermiteGrenade, transform.Find("CardSlotResult"));
                GameObject createdCard = CardGenerator.create_new_card(CardGO, "Stun Grenade", will_card, transform.Find("CardSlotResult"));
            }

            //Basınç + Zehir = Chemical Grenade
            if ((comTypeCard1 == CombineType.Pressure && comTypeCard2 == CombineType.Toxic) || (comTypeCard1 == CombineType.Toxic && comTypeCard2 == CombineType.Pressure))
            {
                Debug.Log("CHEMICA!!!!");

                //Her yere vuruyor
                float[] cardAttackRange = { 0.25f, 0.25f, 0.25f, 0.25f };

                //Yeni bir kart oluşturuyor.
                Card will_card = ScriptableObject.CreateInstance("Card") as Card;

                //Kartın özelliklerini atıyor
                will_card.cardName = "Chemical Grenade";
                will_card.attack = 0;
                will_card.defence = 0;
                will_card.mana = cardMana;
                will_card.attack_range = cardAttackRange;
                will_card.cardDebuff = debuffs.Weakness;

                //CardGenerator.create_new_card(CardGO, "Thermite Grenade", thermiteGrenade, transform.Find("CardSlotResult"));
                GameObject createdCard = CardGenerator.create_new_card(CardGO, "Stun Grenade", will_card, transform.Find("CardSlotResult"));
            }

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
