using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies")]
public class Enemy : ScriptableObject
{
    public string Id, Description;
    public int Health, Mana, Shield;
    public Sprite Character;
    
}
