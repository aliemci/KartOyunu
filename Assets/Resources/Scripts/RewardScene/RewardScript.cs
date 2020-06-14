using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class RewardScript : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Started!");

        Card[] listOfCards = Resources.LoadAll<Card>("Cards");


        for(int i=0; i<3; i++)
        {
            int randomSelection = UnityEngine.Random.Range(0, listOfCards.Length);
            CardGenerator.create_new_card(i.ToString(), listOfCards[i], this.transform);
        }
            

    }

}
