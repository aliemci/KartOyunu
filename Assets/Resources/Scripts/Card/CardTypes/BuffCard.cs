using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum buffs
{
    Adrenaline,
    Alertness,
    Castle,
    Economiser,
    Puffed,
    Resistance,
    Invincible,
    Regenerate,
    None
}

public class BuffCard : MonoBehaviour
{
    public buffs buff;
    public int buffCoefficient;

    public void Start()
    {
        buff = GetComponent<CardDisplay>().card.cardBuff;
        buffCoefficient = GetComponent<CardDisplay>().card.buffCoefficient;
    }

    public void buffApplier(Character effectedCharacter)
    {
        if(!addRepetitionIfBuffInList(buff, effectedCharacter))
        {
            buffQueue helper = new buffQueue();
            helper.buff = buff;
            helper.coefficient = buffCoefficient;
            helper.repetition = 1;
            effectedCharacter.buffList.Add(helper);
        }
    }

    private bool addRepetitionIfBuffInList(buffs buff, Character c)
    {
        foreach (buffQueue item in c.buffList)
        {
            if (item.buff == buff)
            {
                item.repetition++;
                return true;
            }
        }
        return false;
    }



}
