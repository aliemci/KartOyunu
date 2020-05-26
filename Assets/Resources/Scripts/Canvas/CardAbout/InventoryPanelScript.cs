using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelScript : MonoBehaviour
{
    public GameObject cardPrefab;

    public void Start()
    {
        //Üstlerindeki birleşme tuşunu kapatıyor.
        cardPrefab.transform.Find("Combine").gameObject.SetActive(false);

        //Her kart için döngüye giriyor.
        foreach(Card card in GameObject.Find("Inventory").gameObject.GetComponent<InventoryScript>().Inventory)
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
