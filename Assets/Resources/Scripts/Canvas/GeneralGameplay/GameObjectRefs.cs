using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRefs : MonoBehaviour
{
    public GameObject playerGO;
    public GameObject[] rivalGOs;

    void Awake()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        rivalGOs = GameObject.FindGameObjectsWithTag("Enemy");
    }

}
