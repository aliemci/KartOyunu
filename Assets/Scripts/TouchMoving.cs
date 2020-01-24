using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMoving : MonoBehaviour
{


    private void Start()
    {

    }

    private void OnMouseDown()
    {

    }

    private void OnMouseDrag()
    {
        transform.position = Vector3.Lerp(transform.position, Input.GetTouch(0).position, Time.deltaTime * 5);
    }

    private void OnMouseUp()
    {
        
    }
}
