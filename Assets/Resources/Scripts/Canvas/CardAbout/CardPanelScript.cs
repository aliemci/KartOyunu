using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPanelScript : MonoBehaviour
{
    public GameObject cardPrefab;

    public void Start()
    {
        //Üstlerindeki birleşme tuşunu kapatıyor.
        cardPrefab.transform.Find("Combine").gameObject.SetActive(false);

        //Her kart için döngüye giriyor.
        foreach (Card card in GameObject.Find("CardPile").gameObject.GetComponent<CardPileScript>().CardPile)
        {
            //Kartı oluşturuyor ve BG'nin altına atıyor.
            GameObject cardObj = Instantiate(cardPrefab, this.transform.GetChild(0));
            //Oluşturulan şablonun kartını atıyor.
            cardObj.GetComponent<CardDisplay>().card = card;

            
        }

    }

    public void exit()
    {
        Destroy(this.gameObject);
    }
}
