using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    private playerCharacter player;

    public void OnMouseDown()
    {
        //Oyuncuyu bulma
        foreach (playerCharacter pchar in Resources.LoadAll<playerCharacter>("Players"))
        {
            if (pchar.isPlayer)
            {
                player = pchar;
                break;
            }
        }

        //Kartı oyuncuya ekleme
        player.add_card_to_player(this.gameObject.GetComponent<CardDisplay>().card);
    }


}
