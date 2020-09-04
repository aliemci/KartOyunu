using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{

    //Oyuncu kısmı ----------------------------------
    public static void save_player(playerCharacter player)
    {
        string json = JsonUtility.ToJson(player);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PlayerData.txt", json);
    }

    public static playerCharacter load_player()
    {
        if(File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "PlayerData.txt"))
        {
            PlayerData.player = ScriptableObject.CreateInstance<playerCharacter>();
            string json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PlayerData.txt");
            JsonUtility.FromJsonOverwrite(json, PlayerData.player);
        }
        return PlayerData.player;
    }


    //Harita kısmı ----------------------------------
    public static void save_map(Map currentMap)
    {        
        string json = JsonUtility.ToJson(currentMap);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "MapData.txt", json);
    }

    public static Map load_map()
    {
        Map loadedMap = new Map();
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "MapData.txt"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "MapData.txt");
            loadedMap = JsonUtility.FromJson<Map>(json);
            Debug.Log("Harita Yüklendi!");
        }
        else
        {
            Debug.LogWarning("Harita Yüklenemedi! Rastgele Harita oluşturuluyor.");
            loadedMap = random_map();
        }

        return loadedMap;
    }

    public static void new_map()
    {
        save_map(random_map());
        reset_cards();
    }

    private static Map random_map()
    {
        Map tempMap = new Map();

        tempMap.mapSeed = Random.Range(0, 1000);
        tempMap.marketCount = Random.Range(1, 2);
        tempMap.rivalCount = Random.Range(2, 4);
        tempMap.objsOnMap = new List<MapObject>();
        tempMap.isLoaded = false;

        return tempMap;
    }

    private static void reset_cards()
    {
        Card[] cards = Resources.LoadAll<Card>("Cards");
        foreach (Card card in cards)
        {
            card.timesUsed = 0;
            card.canCardUsable = true;
        }
    }
}
