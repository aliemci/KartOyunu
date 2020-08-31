using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : MonoBehaviour
{
    private int mana;
    private int[] attacks;
    private Card card;

    public void Start()
    {
        card = GetComponentInParent<CardDisplay>().card;
        attacks = card.attack_range;
        mana = card.mana;
    }

    public void attack(Character attacker, Character[] defenders, GameObject[] defenderGOs)
    {
        int manaConsumption = Mathf.Abs(mana) + Mathf.Abs(attacker.mana_factor);

        attacker.prepareChances();

        attacker.consumeMana(manaConsumption);


        int i = 0;
        foreach (Character defender in defenders)
        {
            int damage = (attacks[i] + attacker.attack_factor) * attacker.attack_multiplier;
            defender.prepareChances();

            //Eğer saldıran "ölümsüz değilse" ve "şaşırmadıysa" ve "kaçırmadıysa" ve savunan "kaçamadıysa"
            if (!attacker.is_invincible && !attacker.is_confused && !attacker.is_missed && !defender.is_evaded)
            {
                DamageIndicator.CreateDamageIndicator(defenderGOs[i].transform.position, damage);

                defender.takeDamage(damage);
            }

            else
            {
                Debug.Log("Invincible:" + attacker.is_invincible + "\n" +
                    "Confuse:" + attacker.is_confused + "\n" +
                    "Miss:" + attacker.is_missed + "\n" +
                    "Evade:" + defender.is_evaded);
            }

            i++;
        }
        
    }
}
