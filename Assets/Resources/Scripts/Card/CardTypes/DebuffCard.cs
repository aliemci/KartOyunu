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
    public int probablity;
    public int coefficient;
    public int repetition;
    public bool multiplex;

    public void Start()
    {
        debuff = GetComponent<CardDisplay>().card.cardDebuff;
        probablity = GetComponent<CardDisplay>().card.debuffProbablity;
        coefficient = GetComponent<CardDisplay>().card.debuffCoefficient;
        repetition = GetComponent<CardDisplay>().card.debuffRepetition;
        multiplex = GetComponent<CardDisplay>().card.debuffMultiplex;
    }


    public void debuffApplier(Character effectedCharacter)
    {
        //İhtimal ki, buff etkiler ya da etkilemez
        if (Random.Range(0, 100) < probablity)
        {
            //Buraya ekranda bir yazı yazdırma kodu gelmeli. !!!! 

            Debug.Log("Debuff uygulanıyor.");

            if (!addRepetitionIfDebuffInList(debuff, effectedCharacter))
            {
                debuffQueue helper = new debuffQueue();
                helper.debuff = debuff;
                helper.coefficient = coefficient;
                helper.repetition = repetition;
                helper.multiplex = multiplex;

                effectedCharacter.debuffList.Add(helper);
            }
        }
        else
        {
            //Buraya ekranda "KAÇIRDIN" yazdırma kodu gelmeli. !!!! 

            Debug.Log("Debuff kaçtı!");
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
