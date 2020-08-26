using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
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
    Plasma,
    Gase,
    None
}

public class DebuffCard : MonoBehaviour
{
    public debuffs debuff;
    public int debuffCoefficient;

    public void Start()
    {
        debuff = GetComponent<CardDisplay>().card.cardDebuff;
        debuffCoefficient = GetComponent<CardDisplay>().card.debuffCoefficient;
    }

    public void debuffApplier(Character effectedCharacter)
    {
        //Eğer azaltıcılar bu ikisi ise kademeli şekilde etki ediyor. 1'er azalarak tekrar etki etmeli.
        if(debuff == debuffs.Poison || debuff == debuffs.Burn)
        {
            if(!addRepetitionIfDebuffInList(debuff, effectedCharacter))
            {
                debuffQueue helper = new debuffQueue();
                helper.debuff = debuff;
                helper.coefficient = debuffCoefficient;
                helper.repetition = debuffCoefficient;
                effectedCharacter.debuffList.Add(helper);
            }

        }
        else
        {
            if (!addRepetitionIfDebuffInList(debuff, effectedCharacter))
            { 
                debuffQueue helper = new debuffQueue();
                helper.debuff = debuff;
                helper.coefficient = debuffCoefficient;
                helper.repetition = 1;
                effectedCharacter.debuffList.Add(helper);
            }
        }
        

    }


    private bool addRepetitionIfDebuffInList(debuffs debuff, Character c)
    {
        foreach (debuffQueue item in c.debuffList)
        {
            if (item.debuff == debuff)
            {
                item.repetition++;
                return true;
            }
        }
        return false;
    }

}
