using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombineType
{
    Pressure,
    Flame,
    Lightning,
    Toxic
}

public class CombineCard : MonoBehaviour
{
    public CombineType cardCombineType;

    public void Start()
    {
        cardCombineType = GetComponent<CardDisplay>().card.cardCombine;
    }

    
}
