using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [Header("Inventory List")]
    public List<Card> Inventory = new List<Card>();

    [Header("Essentials")]
    public int numberOfCardsInDeck = 5;
    public GameObject Prefab;

    [HideInInspector]
    public GameObject cardPile, CardDeck;
    

    void Awake()
    {
        //Gerekli atamalar
        CardDeck = GameObject.Find("Deck");

        cardPile = GameObject.Find("CardPile");
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

        //Eğer destede kart varsa, yeni kart eklemesin.
        if (CardDeck.GetComponent<DeckScript>().cardsInDeck.Count != 0)
            return;

        //Eğer yeterli sayıda kart kalmadıysa
        if(Inventory.Count < numberOfCardsInDeck)
        {
            foreach(Card card_in_card_pile in cardPile.GetComponent<CardPileScript>().CardPile)
            {
                //Önce envantere ekliyor.
                Inventory.Add(card_in_card_pile);
            }
            //İçindeki bütün elemanları siliyor
            cardPile.GetComponent<CardPileScript>().CardPile.Clear();
        }

        //Limanda olması gereken kart sayısı kadar döngü
        for (int i = 0; i < numberOfCardsInDeck; i++)
        {
            //Rastgele bir tamsayı alıyor.
            int index = Random.Range(0, Inventory.Count);
            //Belirtilmiş özelliklere sahip bir kart oluşturuyor. (Dönüş değeri de var ancak şuan kullanılmıyor.)
            create_new_card(Prefab, "Card " + i.ToString(), Inventory[index], CardDeck.transform);

            //Envanterdeki kart limana ekleniyor.
            CardDeck.GetComponent<DeckScript>().cardsInDeck.Add(Inventory[index]);
            //O kart envanterden siliniyor.
            Inventory.RemoveAt(index);
        }

    }

    //Aldığı girdilere göre yeni bir kart oluşturuyor.
    GameObject create_new_card(GameObject Prefab, string Name, Card card, Transform Parent)
    {
        GameObject createdCard = Instantiate(Prefab);

        createdCard.name = Name;

        //Oluşturulan Gameobject içindeki CardDisplay koduna erişiyor.
        //Koddaki card değişkenine elimizdeki card tipini atıyor.
        createdCard.GetComponent<CardDisplay>().card = card;

        //Gerekli Kodları yüklüyor
        createdCard.AddComponent(System.Type.GetType(card.CardT1.ToString()));

        //Eğer ikincil özellik varsa onun da kodunu yüklüyor.
        if (card.CardT2.ToString() != "None")
            createdCard.AddComponent(System.Type.GetType(card.CardT2.ToString()));

        //Eğer ikincil özelliği birleşme değilse, birleşme tuşunu çalışmaz hale getiriyor.
        if (card.CardT2 != CardType2.CombineCard)
            createdCard.transform.Find("Combine").gameObject.SetActive(false);

        //Bu sayede Deck içine geçip sıralanıyor.
        createdCard.transform.SetParent(Parent);

        return createdCard;
    }
}
