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
        Transform playerDeck = GameObject.Find("Player-deck").transform;
        Transform rivalDeck = GameObject.Find("Enemy-deck").transform;

        foreach(Character character in characters)
        {
            //Karakter eğer player ise
            if(character as playerCharacter != null)
            {
                Debug.Log("PlayerCharacter Found!\n" + character.Id);
                // Karakter oluşturulup atanıyor.
                Transform playerChar = Instantiate(playerPrefab, playerDeck);
                //Gameobject nesnesini alıyor.
                GameObject player = playerChar.gameObject;
                // Layer olarak 9 atanıyor. (Player)
                player.layer = 9;
                // Charcter display koduna atama işlemi. Bu sayede oyuncu nesnesi oluştuktan sonra özelliklerine erişilebilecek
                player.GetComponent<CharacterDisplay>().character = character;
            }
            else
            {
                Debug.Log("rivalCharacter Found!\n" + character.Id);
                // Karakter oluşturulup atanıyor.
                Transform rivalChar = Instantiate(rivalPrefab, rivalDeck);
                //Gameobject nesnesini alıyor.
                GameObject rival = rivalChar.gameObject;
                // Layer olarak 8 atanıyor. (Enemy)
                rival.layer = 8;
                // Charcter display koduna atama işlemi. Bu sayede oyuncu nesnesi oluştuktan sonra özelliklerine erişilebilecek
                rival.GetComponent<CharacterDisplay>().character = character;
            }
        }
    }
}

