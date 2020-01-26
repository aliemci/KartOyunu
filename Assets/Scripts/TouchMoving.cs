using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMoving : MonoBehaviour
{
    Transform originalParent;

    private void Start()
    {
        originalParent = transform.parent;
    }

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
        transform.SetParent(originalParent);
    }
}
