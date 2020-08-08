using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hexagon: MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    public hexagon ownHexagon;

    private GameObject player, playerCamera;

    private Vector3 oldPointerPosition = Vector3.zero;

    void Start()
    {
        player = GameObject.Find("Player");

        playerCamera = player.transform.GetChild(0).gameObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!player.GetComponent<PlayerMovement>().is_camera_dragged)
        {
            Debug.Log("Name:" + this.gameObject.name);
            player.GetComponent<PlayerMovement>().go_to(this.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        player.GetComponent<PlayerMovement>().is_camera_dragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        player.GetComponent<PlayerMovement>().is_camera_dragged = false;
    }
}
