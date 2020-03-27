using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCard : MonoBehaviour
{
    public int Energy;

    public void Start()
    {
        Energy = GetComponentInParent<CardDisplay>().card.mana;
    }
}
