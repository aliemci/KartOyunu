using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffQueue
{
    public buffs buff { get; set; }
    public float coefficient { get; set; }
    public int repetition { get; set; }
}

public class debuffQueue
{
    public debuffs debuff { get; set; }
    public float coefficient { get; set; }
    public int repetition { get; set; }
}

public class Character : ScriptableObject
{
    public string Id;

    public bool
        is_resisted = false,
        is_invincible = false,
        is_stunned = false,
        is_missed = false,
        is_confused = false,
        is_evaded = false;

    public int
        health = 100,
        maxHealth = 100,
        mana = 100,
        shield = 0,
        shield_factor = 0,
        mana_factor = 0,
        attack_factor = 0,
        attack_multiplier = 1;

    public float
        evasion_chance = 0f,
        confused_chance = 0f,
        miss_chance = 0f;

    public List<buffQueue> buffList = new List<buffQueue>();
    public List<debuffQueue> debuffList = new List<debuffQueue>();

    public Sprite CharacterSprite;

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

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    public void consumeMana(int cost)
    {
        mana -= cost;
    }



}

[CreateAssetMenu(fileName = "New Player Character", menuName = "Player Character")]
public class playerCharacter : Character
{
    public bool isPlayer = true;
    
    
}

[CreateAssetMenu(fileName = "New Rival Character", menuName = "Rival Character")]
public class rivalCharacter : Character
{
    public bool isRival = true;

}