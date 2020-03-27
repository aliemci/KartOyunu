using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public enum CardType1
{
    AttackCard,
    DefenceCard,
    EnergyCard,
    BuffCard,
    DebuffCard,
    SpecialCard,
    UnusualCard
}
public enum CardType2
{
    None,
    BuffCard,
    DebuffCard
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject{

    public string cardName;
    
    public CardType1 CardT1;
    [HideInInspector]
    public CardType2 CardT2;

    [HideInInspector]
    public buffs cardBuff;
    [HideInInspector]
    public debuffs cardDebuff;
    

    public int attack, defence, mana;

    public bool isPlayerOwn;
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(Card))]
public class Card_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Card card = (Card)target;

        if (card.CardT1 != CardType1.BuffCard && card.CardT1 != CardType1.DebuffCard)
        {
            card.CardT2 = (CardType2)EditorGUILayout.EnumPopup("Second Card Type", card.CardT2);

            if(card.CardT2 == CardType2.BuffCard)
                card.cardBuff = (buffs)EditorGUILayout.EnumPopup("Buffs", card.cardBuff);
            else if(card.CardT2 == CardType2.DebuffCard)
                card.cardDebuff = (debuffs)EditorGUILayout.EnumPopup("Debuffs", card.cardDebuff);

        }
    }
}
#endif