using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public playerCharacter playerChar;

    public PlayerData(playerCharacter player)
    {
        playerChar = player;
    }
}
