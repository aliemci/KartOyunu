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

    public void attack(Character attacker, Character defender)
    {
        int manaConsumption = Mathf.Abs(Mana) + Mathf.Abs(attacker.mana_factor);
        int damage = (Attack + attacker.attack_factor) * attacker.attack_multiplier;

        attacker.prepareChances();
        defender.prepareChances();

        //Debug.Log("ATTACKER\ninv:" + attacker.is_invincible + "\nconf:" + attacker.is_confused + "\nmiss:" + attacker.is_missed);
        //Debug.Log("DEFENDER\ninv:" + defender.is_invincible + "\nconf:" + defender.is_confused + "\nmiss:" + defender.is_missed);

        //Eğer saldıran "ölümsüz değilse" ve "şaşırmadıysa" ve "kaçırmadıysa" ve savunan "kaçamadıysa"
        if (!attacker.is_invincible && !attacker.is_confused && !attacker.is_missed && !defender.is_evaded)
        {
            defender.takeDamage(damage);
            attacker.consumeMana(manaConsumption);
        }
           
        
    }
}
