using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_turn : MonoBehaviour
{
    [HideInInspector]
    public bool isVariablesDefined = false;

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

            //Player character sınıfının fonksiyonları da çağırılacağı için o tipte atanıyor.
            player = playerObject.GetComponent<CharacterDisplay>().character as playerCharacter;
        }


        //Eğer Combiner açıksa onu kapatsın
        try
        {
            GameObject.Find("CombineCardPanel").GetComponent<CombineWindowMainScript>().window_close();
        }
        catch
        {
            Debug.Log("Combiner açık değil");
        }



        //Oyuncu için fonksiyonlar: Önce sıradaki tur fonksiyonuyla listedeki buff debufflar etkiyecek.
        //Sonra bu değişiklikler yazdırılacak.
        player.next_turn();
        //Debug.Log("Player next turn applied!");
        playerObject.GetComponent<CharacterDisplay>().situationUpdater();
        //Debug.Log("Player situation updated!");

        //Aynı şey her düşman için de kontrol edilecek.
        foreach (GameObject rivalobj in rivalObjects)
        {
            //Debug.Log(rivalobj);
            //rival değişkenini fonksiyonları için bir değişkene atıyor.
            rival = rivalobj.GetComponent<CharacterDisplay>().character as rivalCharacter;
            //rival için temel hazırlıkları yapıyor.
            rival.next_turn();
            //aldığı hasarla ölüp ölmediğine bakılıyor.
            rivalobj.GetComponent<CharacterDisplay>().situationUpdater();

            //Sıra ona geçiyor ve hamlesini oynuyor.
            rival.do_move(player, rivalObjects);
            //Durumu güncelleniyor.
            rivalobj.GetComponent<CharacterDisplay>().situationUpdater();

            //Düşmanı için yazı güncellemeleri
            playerObject.GetComponent<CharacterDisplay>().situationUpdater();
            
        }

        //Kartları yeniden diziyor.
        GameObject.Find("Inventory").GetComponent<InventoryScript>().add_card_to_deck();

    }


    public void end_of_fight()
    {
        //Burada 1 demesinin sebebi şu, bu fonksiyon bir şahıs ölünce tetikleniyor. Ancak o şahıs silinmeden önce tetikleniyor.
        //Yani ölecek bir şahıs hayatta oluyor. O yüzden düşman sayısı 1 iken devam ediyor.
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 1)
        {
            Debug.Log("END OF THE FIGHT");
        }
        else
            Debug.Log("Number of enemies: " + GameObject.FindGameObjectsWithTag("Enemy").Length);
    }
    
}
