using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOverrider : MonoBehaviour
{
    public int sortLayer;

    private void OnValidate()
    {
        GetComponent<Renderer>().sortingOrder = sortLayer;
    }
}
