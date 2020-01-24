using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public new string Name;
    public string Description;

    public int Mana;
    public int Attack;
    public int Health;

    public Sprite Thumbnail;

}
