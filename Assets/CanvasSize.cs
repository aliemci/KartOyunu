using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int h = Screen.height;
        int w = Screen.width;
        Debug.Log(h.ToString() + " :h - w: " +  w.ToString());
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(w,h);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
