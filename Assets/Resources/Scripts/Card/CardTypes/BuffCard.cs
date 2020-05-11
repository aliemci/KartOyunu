using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public float buffCoefficient;

    public void Start()
    {
        buff = GetComponent<CardDisplay>().card.cardBuff;
        buffCoefficient = GetComponent<CardDisplay>().card.buffCoefficient;
    }

    public void buffApplier(Character effectedCharacter)
    {
        buffQueue helper = new buffQueue();
        helper.buff = buff;
        helper.repetition = 1;
        helper.coefficient = buffCoefficient;
        effectedCharacter.buffList.Add(helper);
        
    }

    /*
    private void adrenaline(Character subject)
    {
        buffQueue helper = new buffQueue();
        helper.buff = buffs.Adrenaline;
        helper.coefficient = 1;
        subject.buffList.Add(helper);
        Debug.Log(subject.buffList[0].buff.ToString() + "-" + subject.buffList[0].coefficient.ToString());
    }

    private void alertness(Character subject)
    {
        buffQueue helper = new buffQueue();
        helper.buff = buffs.Alertness;
        helper.coefficient = 1;
        subject.buffList.Add(helper);
        Debug.Log(subject.buffList[0].buff.ToString() + "-" + subject.buffList[0].coefficient.ToString());
    }

    private void castle(Character subject)
    {
        buffQueue helper = new buffQueue();
        helper.buff = buffs.Castle;
        helper.coefficient = 1;
        subject.buffList.Add(helper);
        Debug.Log(subject.buffList[0].buff.ToString() + "-" + subject.buffList[0].coefficient.ToString());
    }

    private void economiser(Character subject)
    {
        buffQueue helper = new buffQueue();
        helper.buff = buffs.Economiser;
        helper.coefficient = 1;
        subject.buffList.Add(helper);
        Debug.Log(subject.buffList[0].buff.ToString() + "-" + subject.buffList[0].coefficient.ToString());
    }

    private void puffed(Character subject)
    {
        buffQueue helper = new buffQueue();
        helper.buff = buffs.Puffed;
        helper.coefficient = 1;
        subject.buffList.Add(helper);
        Debug.Log(subject.buffList[0].buff.ToString() + "-" + subject.buffList[0].coefficient.ToString());
    }
    */

}
