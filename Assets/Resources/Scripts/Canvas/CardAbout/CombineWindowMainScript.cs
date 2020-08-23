using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class CombineWindowMainScript : MonoBehaviour
{
    private GameObject 
        card1, 
        card2;

    private Transform deck;

    public bool 
        isLeftCardSlotEmpty = true, 
        isRightCardSlotEmpty = true;

    private CombineType 
        comTypeCard1, 
        comTypeCard2;

    private Card 
        thermiteGrenade,
        stunGrenade,
        chemicalGrenade,
        Plasma,
        Gase,
        Coma;

    //-------------------------------------


    private void Start()
    {
        deck = GameObject.Find("Deck").transform;
        Debug.Log(deck + " is asssigned");

        thermiteGrenade = Resources.Load<Card>("Cards/Thermite Grenade");
        stunGrenade = Resources.Load<Card>("Cards/Stun Grenade");
        chemicalGrenade = Resources.Load<Card>("Cards/Chemical Grenade");
        //Plasma
        //Gase
        //Come
    }


    public void craft_combined_card()
    {
        //Birleştirilmiş kartları siliyor.
        Destroy(card1);
        Destroy(card2);

        //Oluşmuş kartı limana ekliyor.
        GameObject resultCard = transform.Find("CardSlotResult").GetChild(0).gameObject;
        resultCard.transform.SetParent(deck);
        
        //Kartı kullanılabilir hale getiriyor.
        resultCard.GetComponent<CardDisplay>().toggle_card(true);

        //Kendisini siliyor.
        Destroy(this.gameObject);
    }


    public void Combine_them()
    {
        //Kart kontrolü
        checks_card_slots();

        //Sağ ve solda kartlar varsa
        if (!isLeftCardSlotEmpty && !isRightCardSlotEmpty)
        {
            //Kart oyun nesneleri atanıyor.
            card1 = transform.Find("CardSlotLeft").GetChild(0).gameObject;
            card2 = transform.Find("CardSlotRight").GetChild(0).gameObject;

            //Kart tiplerini alıyor.
            CombineType comTypeCard1 = card1.GetComponent<CardDisplay>().card.cardCombine;
            CombineType comTypeCard2 = card2.GetComponent<CardDisplay>().card.cardCombine;

            //Yeni kartın özellikleri eski iki kartların toplamı kadar oluyor. (Burası değiştirilecek)
            int cardAttack = card1.GetComponent<CardDisplay>().card.attack + card2.GetComponent<CardDisplay>().card.attack;
            int cardDefence = card1.GetComponent<CardDisplay>().card.defence + card2.GetComponent<CardDisplay>().card.defence;
            int cardMana = card1.GetComponent<CardDisplay>().card.mana + card2.GetComponent<CardDisplay>().card.mana;

            //Kart taslağı
            GameObject CardGO = Resources.Load<GameObject>("Prefabs/Card");

            //pressure + Flame = Thermite Grenade
            if ((comTypeCard1 == CombineType.Pressure && comTypeCard2 == CombineType.Flame) || (comTypeCard1 == CombineType.Flame && comTypeCard2 == CombineType.Pressure))
            {
                //Her yere vuruyor
                int[] cardAttackRange = { 1 };

                float burnCoeff = cardAttack / 11; //Trello Kart Kombinasyonu kartında yazılı denkleme göre

                int thermiteMana = cardMana / 2;

                GameObject combinedCard = combined_card_creator("Thermite Grenade", cardAttackRange, 0, thermiteMana, cardMana, debuffs.Burn, burnCoeff, CardGO);

            }

            //pressure + ligthning = Stun Grenade
            else if ((comTypeCard1 == CombineType.Pressure && comTypeCard2 == CombineType.Lightning) || (comTypeCard1 == CombineType.Lightning && comTypeCard2 == CombineType.Pressure))
            {
                //Her yere vuruyor
                int[] cardAttackRange = { 1 };

                float stunCoeff = cardAttack / 20;

                int stunMana = cardMana / 2;

                GameObject combinedCard = combined_card_creator("Stun Grenade", cardAttackRange, 0, stunMana, cardMana, debuffs.Stun, stunCoeff, CardGO);

            }

            //pressure + toxic = Chemical Grenade
            else if ((comTypeCard1 == CombineType.Pressure && comTypeCard2 == CombineType.Toxic) || (comTypeCard1 == CombineType.Toxic && comTypeCard2 == CombineType.Pressure))
            {
                //Her yere vuruyor
                int[] cardAttackRange = { 1 };

                float weaknessCoeff = cardAttack / 11;

                int chemicalMana = cardMana / 2;

                GameObject combinedCard = combined_card_creator("Chemical Grenade", cardAttackRange, 0, chemicalMana, cardMana, debuffs.Weakness, weaknessCoeff, CardGO);
            }

            //Flame + Lightning = Plasma
            else if ((comTypeCard1 == CombineType.Flame && comTypeCard2 == CombineType.Lightning) || (comTypeCard1 == CombineType.Lightning && comTypeCard2 == CombineType.Flame))
            {
                //Her yere vuruyor
                int[] cardAttackRange = { 1 };

                float weaknessCoeff = cardAttack / 11;

                int plasmaMana = cardMana / 2;

                GameObject combinedCard = combined_card_creator("Chemical Grenade", cardAttackRange, 0, plasmaMana, cardMana, debuffs.Poison, weaknessCoeff, CardGO);
            }

            //Flame + Toxic = Gase
            else if ((comTypeCard1 == CombineType.Flame && comTypeCard2 == CombineType.Toxic) || (comTypeCard1 == CombineType.Toxic && comTypeCard2 == CombineType.Flame))
            {
                //Her yere vuruyor
                int[] cardAttackRange = { 1 };

                float weaknessCoeff = cardAttack / 11;

                int gaseMana = cardMana / 2;

                GameObject combinedCard = combined_card_creator("Chemical Grenade", cardAttackRange, 0, gaseMana, cardMana, debuffs.Weakness, weaknessCoeff, CardGO);
            }

            //Ligthning + toxic = Coma
            else if ((comTypeCard1 == CombineType.Lightning && comTypeCard2 == CombineType.Toxic) || (comTypeCard1 == CombineType.Toxic && comTypeCard2 == CombineType.Lightning))
            {
                //Her yere vuruyor
                int[] cardAttackRange = { 1 };

                float weaknessCoeff = cardAttack / 11;

                int comaMana = cardMana / 2;

                GameObject combinedCard = combined_card_creator("Chemical Grenade", cardAttackRange, 0, comaMana, cardMana, debuffs.Weakness, weaknessCoeff, CardGO);
            }

        }
        else
        {
            return;
        }

        

    }


    private GameObject combined_card_creator(string name, int[] AttackRange, int attack, int defence, int mana, debuffs debuff, float debuffCoeff, GameObject cardObject)
    {
        Debug.Log(name + "!!!!");

        //Yeni bir kart oluşturuyor.
        Card willCard = ScriptableObject.CreateInstance("Card") as Card;

        //Kartın özelliklerini atıyor
        willCard.cardName = name;
        willCard.attack = attack;
        willCard.defence = defence;
        willCard.mana = mana;
        willCard.attack_range = AttackRange;
        willCard.cardDebuff = debuff;
        willCard.debuffCoefficient = debuffCoeff;


        GameObject newCard = CardGenerator.create_new_card(name, willCard, transform.Find("CardSlotResult"));

        //Kartı kullanılmaz hale getiriyor.
        newCard.GetComponent<CardDisplay>().toggle_card(false);

        return newCard;
    }


    public void checks_card_slots()
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


    private void return_cards_to_deck()
    {
        //Kartlar kontrol ediliyor.
        checks_card_slots();

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


    public void window_close()
    {
        //Eğer kart yüklendiyse onları geri yerine koyuyor.
        return_cards_to_deck();

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
