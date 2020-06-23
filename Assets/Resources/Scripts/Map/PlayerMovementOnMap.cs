using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOnMap : MonoBehaviour
{
    public GameObject playerObj;

    [SerializeField]
    private MapGenerator mg;

    private void Start()
    {
        mg = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();

        int randomSpawnIndex = Random.Range(0,mg.hexagons.Count);

        Instantiate(playerObj, mg.hexagons[randomSpawnIndex].hexObj.transform);
    }

}
