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


        //Oyuncuyu oluşturma -----------------------------------------------------------------------------------------
        playerCharacter playerChar = SaveSystem.load_player();

        Transform playerCopy = Instantiate(playerPrefab, playerDeck);

        playerCopy.name = "Player";

        GameObject playerGO = playerCopy.gameObject;

        playerGO.layer = 9;

        playerGO.GetComponent<CharacterDisplay>().character = playerChar;

        //Envanterdeki kartları hazırlayacak fonksiyon çağırılıyor.
        inventoryObject.GetComponent<InventoryScript>().inventory_load(playerChar);
        //Oyuncuyu oluşturma sonu ------------------------------------------------------------------------------------


        int indis = 0;
        foreach(Character character in characters)
        {
            indis++;
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

