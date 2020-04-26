using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum debuffs
{
    Poison,
    Confused,
    Stun,
    Blind,
    Frailness,
    Weakness,
    Tired
}

public class DebuffCard : MonoBehaviour
{
    public debuffs debuff;
    public float debuffCoefficient;

    public void Start()
    {
        debuff = GetComponent<CardDisplay>().card.cardDebuff;
        debuffCoefficient = GetComponent<CardDisplay>().card.debuffCoefficient;
    }

    public void debuffApplier(Character effectedCharacter)
    {
        debuffQueue helper = new debuffQueue();
        helper.debuff = debuff;
        helper.repetition = 1;
        helper.coefficient = debuffCoefficient;
        effectedCharacter.debuffList.Add(helper);
        

    }

}
