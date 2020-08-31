using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicate : MonoBehaviour
{    
    private float dilation = 2f;

    TextMeshProUGUI tmp;

    Color textColor;

    private void Awake()
    {
        tmp = this.GetComponent<TextMeshProUGUI>();
        tmp.color = new Color(0, 0, 0, 1) + tmp.color;
        textColor = tmp.color;
    }

    //Yavaş yavaş solduruyor. En sonunda siliyor.
    private void Update()
    {
        this.transform.position += new Vector3(10, 30, 0) * Time.deltaTime;
        dilation -= Time.deltaTime;
        if(dilation < 0)
        {
            textColor.a -= Time.deltaTime;
            tmp.color = textColor;
            if(textColor.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }


}
