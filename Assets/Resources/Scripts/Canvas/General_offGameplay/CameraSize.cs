using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    void Start()
    {
        int h = Screen.height;
        int CamSize = h / 2;
        GetComponent<Camera>().orthographicSize = CamSize;
    }
}
