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

    public bool attack(Character attacker, Character defender)
    {
        int manaConsumption = Mathf.Abs(Mana) + Mathf.Abs(attacker.mana_factor);
        int damage = (Attack + attacker.attack_factor) * attacker.attack_multiplier;

        if (attacker.mana >= manaConsumption)
        {
            defender.health -= damage;
            attacker.mana -= manaConsumption;
            return true;
        }

        return false;
    }
}
