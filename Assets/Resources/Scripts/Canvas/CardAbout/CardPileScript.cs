using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPileScript : MonoBehaviour
{
    public List<Card> CardPile = new List<Card>();

    public GameObject cardPilePanelPrefab;


    public void create_card_pile_panel()
    {
        GameObject card_pile_panel = Instantiate(cardPilePanelPrefab, GameObject.Find("Canvas").transform);
    }

}
