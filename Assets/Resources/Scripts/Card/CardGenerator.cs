using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    Card[] CardTypes;

    public GameObject Prefab;
    
    private void Start()
    {
        /*
        List<Card> inventoryCards = GameObject.Find("Lower-deck").transform.Find("Inventory").GetComponent<InventoryScript>().Inventory;
                
        //Limanda 5 kart olacağı için
        for (int i = 0; i < 5; i++)
        {
            //Rastgele bir tamsayı alıyor.
            int index = Random.Range(0, inventoryCards.Count);
            //Belirtilmiş özelliklere sahip bir kart oluşturuyor. (Dönüş değeri de var ancak şuan kullanılmıyor.)
            create_new_card(Prefab, "Card " + i.ToString(), inventoryCards[index], GameObject.Find("Deck").transform);
                        
        }
        */
                
    }

    //Aldığı girdilere göre yeni bir kart oluşturuyor.
    public static GameObject create_new_card(GameObject Prefab, string Name, Card card, Transform Parent)
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
