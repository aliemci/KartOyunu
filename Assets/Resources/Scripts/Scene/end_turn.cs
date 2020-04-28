using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_turn : MonoBehaviour
{
    private bool isVariablesDefined = false;

    private GameObject playerObject;
    private GameObject[] rivalObjects;

    private playerCharacter player;
    private rivalCharacter rival;


    public void next_turn()
    {
        //Bir kere yapması için var.
        if (!isVariablesDefined)
        {
            rivalObjects = GameObject.FindGameObjectsWithTag("Enemy");

            playerObject = GameObject.FindGameObjectWithTag("Player");

            isVariablesDefined = true;
        }

        try
        {
            //Oyuncu için fonksiyonlar: Önce sıradaki tur fonksiyonuyla listedeki buff debufflar etkiyecek.
            //Sonra bu değişiklikler yazdırılacak.
            player = playerObject.GetComponent<CharacterDisplay>().character as playerCharacter;
            player.next_turn();
            playerObject.GetComponent<CharacterDisplay>().healthManaWriter();
        }
        catch
        {
            if(GameObject.FindGameObjectWithTag("Player") == null)
            {
                Debug.Log("Kaybettin");
                return;
            }
        }

        //Aynı şey her düşman için de kontrol edilecek.
        foreach (GameObject rivalobj in rivalObjects)
        {
            try{
                rival = rivalobj.GetComponent<CharacterDisplay>().character as rivalCharacter;
                rival.next_turn();
                rivalobj.GetComponent<CharacterDisplay>().checkIsDead();
                rival.do_move(player);
            }
            catch {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                {
                    Debug.Log("Bölüm kazanıldı!");
                    return;
                }
            }

        }

        //Kartları yeniden diziyor.
        GameObject.Find("Inventory").GetComponent<InventoryScript>().add_card_to_deck();

    }

    
}
