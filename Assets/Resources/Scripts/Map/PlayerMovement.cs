using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour,  IPointerClickHandler
{
    public hexagon parentHex;

    public MapGenerator mg;

    public Material stdGrass, nbGrass;

    public void Start()
    {
        mg = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<hexagon> neighbours = find_neighbours(parentHex.hexObj);

        foreach (hexagon hex in neighbours)
        {
            hex.hexObj.GetComponent<Renderer>().material.color = new Color(50f,50f,0f);
        }
    }

    public void go_to(GameObject goalHex)
    {
        if(is_neighbour(goalHex))
        {
            this.transform.position = goalHex.transform.position + new Vector3(0f,0f,-1f);
            this.transform.rotation = Quaternion.identity;
            parentHex = goalHex.GetComponent<Hexagon>().ownHexagon;
        }
    }

    bool is_neighbour(GameObject hexGO)
    {
        List<hexagon> neighbours = find_neighbours(parentHex.hexObj);
        foreach (hexagon hex in neighbours)
            if (hex.hexObj == hexGO)
                return true;
        return false;
    }

    List<hexagon> find_neighbours(GameObject playerHex)
    {
        string hexName = playerHex.name;
        int x = (int) playerHex.GetComponent<Hexagon>().ownHexagon.coordinates.x;
        int y = (int) playerHex.GetComponent<Hexagon>().ownHexagon.coordinates.y;


        //neighbours = {x-1, x, x+1}*{y-1, y, y+1}

        Vector2[] neighbours = {
            new Vector2(x + 1, y),
            new Vector2(x, y - 1),
            new Vector2(x, y + 1),
            new Vector2(x - 1, y + 1),
            new Vector2(x - 1, y),
            new Vector2(x - 1, y - 1) };

        List<hexagon> neighbourHexs = new List<hexagon>();

        foreach (hexagon hex in mg.hexagons)
        {
            foreach (Vector2 neighbour in neighbours)
            {
                if (hex.coordinates == neighbour)
                {
                    Debug.Log(neighbour);
                    neighbourHexs.Add(hex);
                }
            }
        }


        return neighbourHexs;
    }
}
