using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    public Transform prefab;

    void Start()
    {
        Transform deck = GameObject.Find("Player-deck").transform;
        Transform playerChar = Instantiate(prefab, deck);    
    }
    
}
