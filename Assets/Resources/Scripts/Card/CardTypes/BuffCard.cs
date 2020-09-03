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
    public int probablity;
    public int coefficient;
    public int repetition;
    public bool multiplex;

    public void Start()
    {
        buff = GetComponent<CardDisplay>().card.cardBuff;
        probablity = GetComponent<CardDisplay>().card.buffProbablity;
        coefficient = GetComponent<CardDisplay>().card.buffCoefficient;
        repetition = GetComponent<CardDisplay>().card.buffRepetition;
        multiplex = GetComponent<CardDisplay>().card.buffMultiplex;
    }

    public void buffApplier(Character effectedCharacter)
    {
        //İhtimal ki, buff etkiler ya da etkilemez
        if (Random.Range(0, 100) < probablity)
        {
            //Buraya ekranda bir yazı yazdırma kodu gelmeli. !!!! 

            Debug.Log("Buff uygulanıyor.");

            if (!addRepetitionIfBuffInList(buff, effectedCharacter))
            {
                buffQueue helper = new buffQueue();
                helper.buff = buff;
                helper.coefficient = coefficient;
                helper.repetition = repetition;
                helper.multiplex = multiplex;

                effectedCharacter.buffList.Add(helper);
            }
        }
        else
        {
            //Buraya ekranda "KAÇIRDIN" yazdırma kodu gelmeli. !!!! 

            Debug.Log("Buff kaçtı!");
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
