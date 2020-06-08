using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public Transform playerPrefab;
    public Transform rivalPrefab;

    public Character[] characters;

    public CharacterGenerator(Transform playerPrefab, Transform rivalPrefab, Character[] characters)
    {
        //Modüler bölüm oluşturucusu yapılırsa buradan yapıya başlanabilir.
    }

    void Start()
    {
        //Limanlar atanıyor.
        Transform playerDeck = GameObject.Find("Player-deck").transform;
        Transform rivalDeck = GameObject.Find("Enemy-deck").transform;

        GameObject inventoryObject = GameObject.Find("Inventory").gameObject;

        int indis = 0;
        foreach(Character character in characters)
        {
            indis++;
            //Karakter eğer player ise
            if(character as playerCharacter != null)
            {
                // Karakter oluşturulup atanıyor.
                Transform playerChar = Instantiate(playerPrefab, playerDeck);
                playerChar.name = "Player";
                //Gameobject nesnesini alıyor.
                GameObject player = playerChar.gameObject;
                // Layer olarak 9 atanıyor. (Player)
                player.layer = 9;
                // Charcter display koduna atama işlemi. Bu sayede oyuncu nesnesi oluştuktan sonra özelliklerine erişilebilecek
                player.GetComponent<CharacterDisplay>().character = Instantiate(character);
                //Envanterdeki kartları hazırlayacak fonksiyon çağırılıyor.
                inventoryObject.GetComponent<InventoryScript>().inventory_load(character as playerCharacter);
            }
            else
            {
                // Karakter oluşturulup atanıyor.
                Transform rivalChar = Instantiate(rivalPrefab, rivalDeck);
                rivalChar.name = "Enemy " + indis;
                //Gameobject nesnesini alıyor.
                GameObject rival = rivalChar.gameObject;
                // Layer olarak 8 atanıyor. (Enemy)
                rival.layer = 8;
                // Charcter display koduna atama işlemi. Bu sayede oyuncu nesnesi oluştuktan sonra özelliklerine erişilebilecek
                rival.GetComponent<CharacterDisplay>().character = Instantiate(character);
            }
        }
    }
}

