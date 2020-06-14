using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    Card[] CardTypes;

    GameObject prefab;


    //Aldığı girdilere göre yeni bir kart oluşturuyor.
    public static GameObject create_new_card(string Name, Card card, Transform Parent)
    {
        //Kartı kendisi asset/resources/prefabs dizininden buluyor.
        GameObject Prefab = Resources.Load("Prefabs/Card") as GameObject;

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
