using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
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
            Debug.Log(loadedMap.mapSeed);
        }
        else
        {
            loadedMap.mapSeed = Random.Range(0, 1000);
            loadedMap.marketCount = Random.Range(0, 1);
            loadedMap.rivalCount = Random.Range(2, 4);
        }

        return loadedMap;
    }

}
