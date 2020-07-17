using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour,  IPointerClickHandler
{
    public hexagon parentHex;

    //[HideInInspector]
    public bool is_camera_dragged;

    public MapGenerator mg;

    public Material stdGrass, nbGrass;

    public void Start()
    {
        mg = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
        //List<hexagon> neighbours = find_neighbours(parentHex.hexObj);

        //var colorBlock = new MaterialPropertyBlock();

        //colorBlock.SetColor("_BaseColor", new Color(0f,0.9f,0f,0.8f));

        //foreach (hexagon hex in neighbours)
        //{
        //    hex.hexObj.GetComponent<Renderer>().SetPropertyBlock(colorBlock);
        //}
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

        List<hexagon> neighbourHexs = new List<hexagon>();
        Vector2[] neighbours = new Vector2[6];

        //Tek ise
        if (y%2 == 0)
            neighbours = new Vector2[]{
                new Vector2(x + 1, y),
                new Vector2(x, y - 1),
                new Vector2(x, y + 1),
                new Vector2(x - 1, y + 1),
                new Vector2(x - 1, y),
                new Vector2(x - 1, y - 1)
            };
        //Çift ise
        else
            neighbours = new Vector2[]{
                new Vector2(x - 1, y),
                new Vector2(x, y - 1),
                new Vector2(x, y + 1),
                new Vector2(x + 1, y - 1),
                new Vector2(x + 1, y),
                new Vector2(x + 1, y + 1)
            };

        foreach (hexagon hex in mg.hexagons)
        {
            foreach (Vector2 neighbour in neighbours)
            {
                if (hex.coordinates == neighbour)
                {
                    //Debug.Log(neighbour);
                    neighbourHexs.Add(hex);
                }
            }
        }

        return neighbourHexs;
    }

}
