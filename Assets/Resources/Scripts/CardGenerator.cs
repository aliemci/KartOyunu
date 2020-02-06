using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    Card[] CardTypes;
    
    public GameObject Prefab;
    
    void Awake()
    {
        //Card_Types klasörünün altındaki bütün Card tipindeki nesneleri alıyor.
        CardTypes = Resources.LoadAll<Card>("Card_Types");

        foreach (Card card_item in CardTypes)
        {
            //Öncelikle prefab ile Gameobject oluşturuluyor.
            GameObject createdCard = Instantiate(Prefab);

            //Oluşturulan Gameobject içindeki CardDisplay koduna erişiyor.
            //Koddaki card değişkenine elimizdeki card tipini atıyor.
            createdCard.GetComponent<CardDisplay>().card = card_item;
            
            //Yeni oluşturulmuş OyunNesnesi'nin ebeveyni olarak "Deck"i veriyor.
            //Bu sayede Deck içine geçip sıralanıyor.
            createdCard.transform.SetParent(GameObject.Find("Deck").transform);
            //createdCard.GetComponent<CardDisplay>().Refresh();
        }

    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
