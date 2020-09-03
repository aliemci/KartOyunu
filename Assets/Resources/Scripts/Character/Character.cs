using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Karaktere gelecek arttırıcıları bir listede tutuyor. Bunlar sıra bazlı uygulanabiliyor.
[System.Serializable]
public class buffQueue
{
    public buffs buff;
    public int coefficient;
    public int probablity;
    public int repetition;
    public bool multiplex;
}

//Aynı şekilde azaltıcılar da uygulanabiliyor.
[System.Serializable]
public class debuffQueue
{
    public debuffs debuff;
    public int coefficient;
    public int probablity;
    public int repetition;
    public bool multiplex;
}


//-----------------------------------------------

public abstract class Character : ScriptableObject
{
    //--------------- DEĞİŞKENLER ---------------
    [Header("Name")]
    public string Id;

    [Header("General Attributes")]

    [Tooltip("Can miktarı")]
    public int health = 100;
    [Tooltip("En yüksek can miktarı")]
    public int maxHealth = 100;
    [Tooltip("Enerji miktarı")]
    public int mana = 100;
    [Tooltip("Kalkan miktarı")]
    public int shield = 0;
    

    //[HideInInspector]
    public bool
        is_resisted = false,
        is_invincible = false,
        is_stunned = false,
        is_blinded = false,
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
        blind_chance = 0f,
        miss_chance = 0f;

    [Header("Buff & Debuff List")]
    //[HideInInspector]
    public List<buffQueue> buffList = new List<buffQueue>();
    //[HideInInspector]
    public List<debuffQueue> debuffList = new List<debuffQueue>();


    [Header("Style")]
    public Sprite CharacterSprite;
    
    
    //--------------- FONKSİYONLAR ---------------

    //Yeni tura geçildiğinde bu fonksiyon tetikleniyor.
    public void next_turn()
    {
        //Buff
        for (int i = 0; i < buffList.Count; i++)
        {
            if(buffList[i].repetition > 0)
            {
                if (buffList[i].multiplex)
                {
                    doBuff(buffList[i].buff, buffList[i].repetition);
                    buffList[i].repetition--;
                }
                else
                {
                    doBuff(buffList[i].buff, buffList[i].coefficient);
                    buffList[i].repetition--;
                }
            }
            else
            {
                resetBuff(buffList[i].buff);
                buffList.Remove(buffList[i]);
            }
        }

        //Debuff
        for (int i = 0; i < debuffList.Count; i++)
        {
            if (debuffList[i].repetition > 0)
            {
                if (debuffList[i].multiplex)
                {
                    doDebuff(debuffList[i].debuff, debuffList[i].repetition);
                    debuffList[i].repetition--;
                }
                else
                {
                    doDebuff(debuffList[i].debuff, debuffList[i].coefficient);
                    debuffList[i].repetition--;
                }
            }
            else
            {
                resetDebuff(debuffList[i].debuff);
                debuffList.Remove(debuffList[i]);
            }
        }

    }
    
    //Arttırıcı uygulayıcısı (Bu ekleme için değil!)
    public void doBuff(buffs buff, float coefficient)
    {
        switch (buff)
        {
            case buffs.Adrenaline:
                attack_multiplier = 2; //Her zaman iki katına çıkartıyor.
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

            case buffs.Regenerate:
                health += Mathf.RoundToInt(coefficient);
                break;

        }
    }

    //Azaltıcı uygulayıcısı (Bu ekleme için değil!)
    public void doDebuff(debuffs debuff, float coefficient)
    {
        switch (debuff)
        {
            case debuffs.Poison:
                health -= Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Burn:
                health -= Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Confused:
                confused_chance = Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Stun:
                is_stunned = true;
                break;

            case debuffs.Blind:
                blind_chance = Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Frailness:
                shield_factor = Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Weakness:
                attack_factor -= Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Tired:
                mana_factor = Mathf.RoundToInt(coefficient);
                break;

            case debuffs.Plasma:
                if (Random.Range(0f, 100f) < 30)
                    is_stunned = true;
                break;

            case debuffs.Gase:
                if (Random.Range(0f, 100f) < 50)
                    attack_factor -= Mathf.RoundToInt(coefficient);
                break;

        }
    }

    public void resetBuff(buffs buff)
    {
        switch (buff)
        {
            case buffs.Adrenaline:
                attack_multiplier = 1;
                break;

            case buffs.Alertness:
                evasion_chance = 0;
                break;

            case buffs.Castle:
                shield_factor = 0;
                break;

            case buffs.Economiser:
                mana_factor = 0;
                break;

            case buffs.Puffed:
                attack_factor = 0;
                break;

            case buffs.Resistance:
                is_resisted = false;
                break;

            case buffs.Invincible:
                is_invincible = false;
                break;

            case buffs.Regenerate:
                break;
        }
    }

    public void resetDebuff(debuffs debuff)
    {
        switch (debuff)
        {
            case debuffs.Poison:
                break;

            case debuffs.Burn:
                break;

            case debuffs.Confused:
                confused_chance = 0;
                break;

            case debuffs.Stun:
                is_stunned = false;
                break;

            case debuffs.Blind:
                blind_chance = 0;
                break;

            case debuffs.Frailness:
                shield_factor = 0;
                break;

            case debuffs.Weakness:
                break;

            case debuffs.Tired:
                mana_factor = 0;
                break;

            case debuffs.Plasma:
                break;

            case debuffs.Gase:
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

        if (Random.Range(0f, 100f) < blind_chance)
            is_blinded = true;
        else
            is_blinded = false;
    }

    //Aldığı hasarı uygulayıcı
    public void takeDamage(int damage)
    {
        if (shield >= damage)
        {
            shield -= damage;
        }
        else
        {
            health -= damage - shield;
            shield = 0;
        }
    }

    //Kullandığı enerjiyi uygulayıcı
    public void consumeMana(int cost)
    {
        mana -= cost - mana_factor;
    }

    //Kalkan uygulayıcı
    public void shieldApply(int amount)
    {
        shield += amount + shield_factor;
    }

    //Can uygulayıcı
    public void healApply(int amount)
    {
        health += amount;

        //Azami can sınırına ulaşırsa, aşmasın.
        if (health > maxHealth)
            health = maxHealth;
    }

    //Enerji uygulayıcısı
    public void energyApply(int amount)
    {
        shield += amount;
    }

    //-----------------------------------------------

}



