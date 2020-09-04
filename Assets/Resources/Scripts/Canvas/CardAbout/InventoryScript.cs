using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [Header("Inventory List")]
    public List<Card> Inventory = new List<Card>();

    private List<Card> DeckList = new List<Card>();
    private List<Card> CardPileList = new List<Card>();


    [Header("Essentials")]
    public const int numberOfCardsInDeck = 5;
    public GameObject cardPrefab;
    public GameObject inventoryPanelPrefab;

    [HideInInspector]
    public GameObject cardPile, CardDeck;
    

    void Awake()
    {
        //Gerekli atamalar
        CardDeck = GameObject.Find("Deck");

        cardPile = GameObject.Find("CardPile");

        DeckList = CardDeck.GetComponent<DeckScript>().cardsInDeck;

        CardPileList = cardPile.GetComponent<CardPileScript>().CardPile;
    }

    public void inventory_load(playerCharacter player)
    {
        //Karakterin başlangıç kartlarını liste halinde alıyor.        
        List<starterCards> playerStarterCards = player.StartingCards;

        //Her kart için döngüye giriyor.
        for (int i = 0; i < playerStarterCards.Count; i++)
        {
            //Her karttan kaç tane varsa onu bir değişkene atıyor.
            int number = playerStarterCards[i].count;

            //Kart adedince döngüye giriyor.
            for (int j = 0; j < number; j++)
                //Bunları envantere ekliyor.
                Inventory.Add(playerStarterCards[i].card);

        }

        //Limana kart ekleme fonksiyonunu çağırıyor.
        add_card_to_deck();
        
    }

    public void add_card_to_deck() {

        cleanDeck();

        //Eğer envanterde yeterli sayıda kart kalmadıysa
        if (Inventory.Count < numberOfCardsInDeck)
        {
            cardPileToInventory();
        }

        InventoryToDeck();

        Debug.Log("-------------------------------");

        //Eğer ki limanda yeteri kadar kart yoksa
        if(CardDeck.GetComponent<DeckScript>().cardsInDeck.Count < numberOfCardsInDeck)
        {
            cardPileToInventory();
            InventoryToDeck();
        }


    }

    public void create_inventory_panel()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        //for (int i = 0; i < canvas.childCount; i++)
        //{
        //    
        //}
        GameObject inventory_panel = Instantiate(inventoryPanelPrefab, GameObject.Find("Canvas_2").transform);
    }
    
    private void cleanDeck()
    {
        //Eğer destede kart varsa, yeni kart eklemesin.
        if (DeckList.Count != 0)
        {
            //Destede kalan kartlar
            foreach (Card card in DeckList)
            {
                //kullanılmışların içine atıyor.
                CardPileList.Add(card);
            }

            //Destedeki kartların vekillerini yok etsin.
            DeckList.Clear();

            //Destede bulunan kartları yok etsin.
            for (int i = 0; i < CardDeck.transform.childCount; i++)
            {
                Destroy(CardDeck.transform.GetChild(i).gameObject);
            }

        }
    }

    private void cardPileToInventory()
    {
        //Kullanılmış kartlar listesinden alıyor.
        foreach (Card card_in_card_pile in cardPile.GetComponent<CardPileScript>().CardPile)
        {
            //Önce envantere ekliyor.
            Inventory.Add(card_in_card_pile);
        }
        //İçindeki bütün elemanları siliyor
        cardPile.GetComponent<CardPileScript>().CardPile.Clear();
    }

    private void InventoryToDeck()
    {
        shortcuts.Shuffle(Inventory);

        int InventoryLength = Inventory.Count;
        int InventoryIndex = 0;
        for (int i = 0; i < InventoryLength; i++)
        {
            Debug.Log("Inventory.Count " + Inventory.Count);
            Debug.Log("Inventory " + InventoryIndex + " is " + Inventory[InventoryIndex].name + "\t → Can usable:" + Inventory[InventoryIndex].canCardUsable);
            if (Inventory[InventoryIndex].canCardUsable)
            {
                //Belirtilmiş özelliklere sahip bir kart oluşturuyor. (Dönüş değeri de var ancak şuan kullanılmıyor.)
                CardGenerator.create_new_card("Card " + i.ToString(), Inventory[InventoryIndex], CardDeck.transform);

                //Envanterdeki kart limana ekleniyor.
                CardDeck.GetComponent<DeckScript>().cardsInDeck.Add(Inventory[InventoryIndex]);
                //O kart envanterden siliniyor.
                Inventory.RemoveAt(InventoryIndex);
            }
            else
            {
                InventoryIndex++;
            }

            //Eğer limanda yeteri kart varsa durdursun
            if (CardDeck.GetComponent<DeckScript>().cardsInDeck.Count == numberOfCardsInDeck)
                break;
        }
    }

}
