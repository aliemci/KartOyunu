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


}
