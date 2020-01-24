using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMoving : MonoBehaviour
{
    private void OnMouseDown()
    {

    }

    private void OnMouseDrag()
    {
        transform.position = Input.GetTouch(0).position;
    }

    private void OnMouseUp()
    {
        
    }
}
