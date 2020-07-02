using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hexagon: MonoBehaviour, IPointerClickHandler
{
    public hexagon ownHexagon;

    void Start()
    {

    } 


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
    }

}
