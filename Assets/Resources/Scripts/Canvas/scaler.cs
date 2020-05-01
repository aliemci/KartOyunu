using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{

    public Transform ParentScaler, ChildScaler;

    float resolution_scale;
    Vector2 base_resolution = new Vector2(1334, 750);
    Vector2 resolution;
    

    void Start()
    {
        resolution_scale = GameObject.Find("Canvas").GetComponent<CanvasSize>().canvas_scaler;
        //Debug.Log(resolution_scale);
        GetComponent<RectTransform>().sizeDelta *= resolution_scale;
        //Debug.Log(GetComponent<RectTransform>().sizeDelta);
    }

}
