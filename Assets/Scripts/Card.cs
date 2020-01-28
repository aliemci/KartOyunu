using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public new string id;
    public string Description;
    
    public int Health, Mana, Attack;

    public Sprite Thumbnail;

    public bool isPlayerOwn;
}
