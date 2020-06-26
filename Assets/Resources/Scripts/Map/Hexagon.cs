using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hexagon: MonoBehaviour, IPointerClickHandler
{

    public hexagon ownHexagon;
    public PlayerMovement pm;

    void Start()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();

    } 


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);

        GameObject goalHexagon = eventData.pointerCurrentRaycast.gameObject;

        pm.go_to(goalHexagon);
    }

}
