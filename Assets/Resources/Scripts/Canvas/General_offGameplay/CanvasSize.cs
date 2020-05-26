using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSize : MonoBehaviour
{

    public float canvas_scaler;


    void Awake()
    {
        canvas_scaler = GetComponent<RectTransform>().localScale.x;
    }

    void Start()
    {
        int h = Screen.height;
        int w = Screen.width;
        //Debug.Log(h.ToString() + " :h - w: " +  w.ToString());
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(w,h);
    }
    
}
