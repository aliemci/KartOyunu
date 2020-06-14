using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.UI;

public class RewardScript : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Started!");

        Card[] listOfCards = Resources.LoadAll<Card>("Cards");


        for(int i=0; i<3; i++)
        {
            int randomSelection = UnityEngine.Random.Range(0, listOfCards.Length);
            GameObject rewardCard = CardGenerator.create_new_card(i.ToString(), listOfCards[randomSelection], this.transform);
            rewardCard.transform.Find("Combine").gameObject.SetActive(false);
            rewardCard.GetComponent<TouchMoving>().enabled = false;

            rewardCard.AddComponent<RewardButton>();

        }
            

    }

}
