using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public float CloudSpeed;

    private Vector3 firstPosition;

    void Start()
    {
        firstPosition = transform.position;    
    }

    void Update()
    {
        transform.position -= new Vector3(CloudSpeed, 0f, 0f);

        if (transform.position.x < 0 - 245)
            transform.position = firstPosition;
    }
}
