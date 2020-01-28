using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMoving : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    Vector2 grabOffset;

/*
    private void OnMouseDown()
    {
        transform.SetParent(transform.parent.parent);
    }

    private void OnMouseDrag()
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            transform.position = Vector3.Lerp(transform.position, Input.GetTouch(0).position, Time.deltaTime * 5);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition, Time.deltaTime * 5);
        }
    }

    private void OnMouseUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up, 500f);
        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<EnemyDisplay>().DamageTaken(GetComponent<CardDisplay>().card.Attack);
        }
        transform.SetParent(originalParent);

    }

*/


    public void OnBeginDrag(PointerEventData eventData)
    {
        grabOffset = Input.mousePosition - transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.parent.parent);
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            transform.position = Vector3.Lerp(transform.position, Input.GetTouch(0).position + grabOffset, Time.deltaTime * 5);
        }
        else
        {
            Vector3 grabOffset3 = grabOffset;
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition + grabOffset3, Time.deltaTime * 5);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up, 500f);
        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<EnemyDisplay>().DamageTaken(GetComponent<CardDisplay>().card.Attack);
        }
        transform.SetParent(originalParent);
    }
}
