using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardButton : MonoBehaviour
{
    public void OnMouseDown()
    {
        //Kartı oyuncuya ekleme
        PlayerData.player.add_card_to_player(this.gameObject.GetComponent<CardDisplay>().card);

        SaveSystem.save_player(PlayerData.player);

        //Belki buraya animasyon eklenebilir.

        //Haritaya dönüş yapmak için.
        SceneManager.LoadScene(2);
    }


}
