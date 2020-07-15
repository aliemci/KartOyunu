using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hexagon: MonoBehaviour, IPointerClickHandler
{
    public hexagon ownHexagon;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    } 


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);

        player.GetComponent<PlayerMovement>().go_to(this.gameObject);
    }

}
