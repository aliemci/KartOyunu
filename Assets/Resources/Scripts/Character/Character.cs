using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffQueue
{
    public buffs buff { get; set; }
    public int coefficient { get; set; }
}

public class debuffQueue
{
    public debuffs debuff { get; set; }
    public int coefficient { get; set; }
}

public class Character : ScriptableObject
{
    public string Id;

    public bool
        is_resisted = false,
        is_invincible = false,
        is_stunned = false;

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