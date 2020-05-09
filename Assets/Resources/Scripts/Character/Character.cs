using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Karaktere gelecek arttırıcıları bir listede tutuyor. Bunlar sıra bazlı uygulanabiliyor.
[System.Serializable]
public class buffQueue
{
    public buffs buff { get; set; }
    public float coefficient { get; set; }
    public int repetition { get; set; }
}

//Aynı şekilde azaltıcılar da uygulanabiliyor.
[System.Serializable]
public class debuffQueue
{
    public debuffs debuff { get; set; }
    public float coefficient { get; set; }
    public int repetition { get; set; }
}


//-----------------------------------------------

public abstract class Character : ScriptableObject
{
    [Header("Name")]
    public string Id;

    [Header("General Attributes")]
    public int health = 100;
    public int maxHealth = 100;
    public int mana = 100;
    public int shield = 0;

    //[HideInInspector]
    public bool
        is_resisted = false,
        is_invincible = false,
        is_stunned = false,
        is_missed = false,
        is_confused = false,
        is_evaded = false;

    //[HideInInspector]
    public int
        shield_factor = 0,
        mana_factor = 0,
        attack_factor = 0,
        attack_multiplier = 1;

    //[HideInInspector]
    public float
        evasion_chance = 0f,
        confused_chance = 0f,
        miss_chance = 0f;

    [Header("Buff & Debuff List")]
    public List<buffQueue> buffList = new List<buffQueue>();
    public List<debuffQueue> debuffList = new List<debuffQueue>();


    [Header("Style")]
    public Sprite CharacterSprite;

    //-----------------------------------------------

    //Yeni tura geçildiğinde bu fonksiyon tetikleniyor.
    public void next_turn()
    {
        if (buffList.Count > 0)
        {
            //Buff etkisi uygulanıyor.
            doBuff(buffList[0].buff, buffList[0].coefficient, buffList[0].repetition);
            buffList.Remove(buffList[0]);
        }

        if(debuffList.Count > 0)
        {
            //Debuff etkisi uygulanıyor.
            doDebuff(debuffList[0].debuff, debuffList[0].coefficient, debuffList[0].repetition);
            debuffList.Remove(debuffList[0]);
        }
        
    }
    
    //Arttırıcı uygulayıcısı
    public void doBuff(buffs buff, float coefficient, int repetition = 1)
    {
        switch (buff)
        {
            case buffs.Adrenaline:
                attack_multiplier = Mathf.RoundToInt(coefficient);
                break;

            case buffs.Alertness:
                evasion_chance = coefficient;
                break;

            case buffs.Castle:
                shield_factor = Mathf.RoundToInt(coefficient);
                break;

            case buffs.Economiser:
                mana_factor = Mathf.RoundToInt(coefficient);
                break;

            case buffs.Puffed:
                attack_factor = Mathf.RoundToInt(coefficient);
                break;

            case buffs.Resistance:
                is_resisted = true;
                break;

            case buffs.Invincible:
                is_invincible = true;
                break;

        }
    }

    //Azaltıcı uygulayıcısı
    public void doDebuff(debuffs debuff, float coefficient, int repetition = 1)
    {
        switch (debuff)
        {
            case debuffs.Poison:
                health -= Mathf.RoundToInt(coefficient) * repetition;
                break;

            case debuffs.Confused:
                confused_chance = Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Frailness:
                shield_factor = Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Stun:
                is_stunned = true;
                break;

            case debuffs.Tired:
                mana_factor = Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Weakness:
                attack_factor = Mathf.RoundToInt(coefficient);
                break;
        }
    }

    //İhtimalleri hesaplıyor. Ona göre değişkenleri düzenliyor.
    public void prepareChances()
    {
        if (Random.Range(0f, 100f) < confused_chance)
            is_confused = true;
        else
            is_confused = false;

        if (Random.Range(0f, 100f) < miss_chance)
            is_missed = true;
        else
            is_missed = false;

        if (Random.Range(0f, 100f) < evasion_chance)
            is_evaded = true;
        else
            is_evaded = false;
    }

    //Aldığı hasarı uygulayıcı
    public void takeDamage(int damage)
    {
        health -= damage;
    }

    //Kullandığı enerjiyi uygulayıcı
    public void consumeMana(int cost)
    {
        mana -= cost;
    }

    //Kalkan uygulayıcı
    public void shieldApply(int amount)
    {
        shield += amount;
    }

    //-----------------------------------------------

}



