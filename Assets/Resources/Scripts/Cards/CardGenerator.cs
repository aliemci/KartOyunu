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
        //CardTypes klasörünün altındaki bütün Card tipindeki nesneleri alıyor.
        CardTypes = Resources.LoadAll<Card>("CardTypes");
        

        //Oyuncunun sahip olduğu kartlar bir listede toplanacak
        List<Card> PlayerCards = new List<Card>();
        

        foreach (Card kart in CardTypes)
        {
            if(kart.isPlayerOwn == true)
            {
                PlayerCards.Add(kart);
            }
        }
        

        //Limanda 5 kart olacağı için
        for (int i = 0; i < 5; i++)
        {
            //Rastgele bir tamsayı alıyor.
            int index = Random.Range(0, CardTypes.Length);

            //Öncelikle prefab ile Gameobject oluşturuluyor.
            GameObject createdCard = Instantiate(Prefab);

            //Oluşturulan Gameobject içindeki CardDisplay koduna erişiyor.
            //Koddaki card değişkenine elimizdeki card tipini atıyor.
            createdCard.GetComponent<CardDisplay>().card = CardTypes[index];
            
            //Yeni oluşturulmuş OyunNesnesi'nin ebeveyni olarak "Deck"i veriyor.
            //Bu sayede Deck içine geçip sıralanıyor.
            createdCard.transform.SetParent(GameObject.Find("Deck").transform);
        }
        
        
    }
    
}
