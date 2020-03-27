using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : MonoBehaviour
{
    public int Attack, Mana;

    public void Start()
    {
        Attack = GetComponentInParent<CardDisplay>().card.attack;
        Mana = GetComponentInParent<CardDisplay>().card.mana;
    }
}
