using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelScript : MonoBehaviour
{
    public GameObject cardPrefab;

    private GameObject canvas;

    public void Start()
    {
        //Üstlerindeki birleşme tuşunu kapatıyor.
        cardPrefab.transform.Find("Combine").gameObject.SetActive(false);
        //Kartların haraket etmesini engellemek için kodu kapatıyor.
        cardPrefab.GetComponent<TouchMoving>().enabled = false;

        //Her kart çeşidinden bir tane alması için yapıldı.
        //var uniqueCards = new HashSet<Card>(GameObject.Find("Inventory").gameObject.GetComponent<InventoryScript>().Inventory);

        //Her kart için döngüye giriyor.
        foreach(Card card in GameObject.Find("Inventory").gameObject.GetComponent<InventoryScript>().Inventory)
        {
            //Kartı oluşturuyor ve BG'nin altına atıyor.
            GameObject cardObj = Instantiate(cardPrefab, this.transform.Find("Container").GetChild(0));
            //Oluşturulan şablonun kartını atıyor.
            cardObj.GetComponent<CardDisplay>().card = card;
        }
        
        //Canvas'ı kapatıyor.
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
    }

    public void exit()
    {
        //Canvas'ı açıyor.
        canvas.SetActive(true);

        //Üstlerindeki birleşme tuşunu açıyor.
        cardPrefab.transform.Find("Combine").gameObject.SetActive(true);
        //Kartların haraket etmesini sağlamak için kodu açıyor.
        cardPrefab.GetComponent<TouchMoving>().enabled = true;

        //Kendini kapatıyor.
        Destroy(this.gameObject);
    }
}
