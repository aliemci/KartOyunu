using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinerWindowMainScript : MonoBehaviour
{
    GameObject card1, card2;
    Transform deck;

    private void Start()
    {
        deck = GameObject.Find("Lower-deck").transform.Find("Card-deck").GetChild(0).transform;
        Debug.Log(deck + " is asssigned");
    }

    
    void CombineThem()
    {

    }


    void returnCardsToDeck()
    {
        if (this.transform.Find("CardSlotLeft").transform.childCount > 0)
        {
            card1 = this.transform.Find("CardSlotLeft").transform.GetChild(0).gameObject;
            card1.transform.SetParent(deck);
        }

        if (this.transform.Find("CardSlotRight").transform.childCount > 0)
        {
            card2 = this.transform.Find("CardSlotRight").transform.GetChild(0).gameObject;
            card2.transform.SetParent(deck);
        }

    }


    public void windowClose()
    {
        //Eğer kart yüklendiyse onları geri yerine koyuyor.
        returnCardsToDeck();

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
