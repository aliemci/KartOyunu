using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceCard : MonoBehaviour
{
    public int Defence, Mana;

    public void Start()
    {
        Defence = GetComponentInParent<CardDisplay>().card.defence;
        Mana = GetComponentInParent<CardDisplay>().card.mana;
    }
}
