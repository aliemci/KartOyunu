using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int h = Screen.height;
        int CamSize = h / 2;
        GetComponent<Camera>().orthographicSize = CamSize;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
