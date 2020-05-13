using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum debuffs
{
    Poison,
    Burn,
    Confused,
    Stun,
    Blind,
    Frailness,
    Weakness,
    Tired,
    None
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
        //Eğer azaltıcılar bu ikisi ise kademeli şekilde etki ediyor. 1'er azalarak tekrar etki ediyor.
        if(debuff == debuffs.Poison || debuff == debuffs.Burn)
        {
            for(int i=0; i<debuffCoefficient; i++)
            {
                debuffQueue helper = new debuffQueue();
                helper.debuff = debuff;
                helper.coefficient = debuffCoefficient - i;
                effectedCharacter.debuffList.Add(helper);
            }
        }
        else
        {
            debuffQueue helper = new debuffQueue();
            helper.debuff = debuff;
            helper.coefficient = debuffCoefficient;
            effectedCharacter.debuffList.Add(helper);
        }
        

    }

}
