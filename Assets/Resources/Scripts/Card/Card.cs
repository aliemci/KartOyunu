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
    DebuffCard,
    CombineCard
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
    [HideInInspector]
    public CombineType cardCombine;

    [HideInInspector]
    public float buffCoefficient;
    [HideInInspector]
    public float debuffCoefficient;

    [HideInInspector]
    public int attack, defence, mana;

    public bool isPlayerOwn;
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(Card))]
public class Card_Editor : Editor
{
    //Düzenleme alanındaki her eylemden sonra bu fonksiyon çağırılıyor.
    public override void OnInspectorGUI()
    {
        //Öntanımlı  çizilenler için.
        DrawDefaultInspector();

        //Üzerinde değişiklik yaptığımız nesnenin kart olduğunu belirterek değişkene atıyoruz.
        Card card = (Card)target;

        //Eğer ilk kart özelliği buff&debuff değilse, ikinci özelliği görünür kılıyoruz.
        if (card.CardT1 != CardType1.BuffCard && card.CardT1 != CardType1.DebuffCard)
        {
            //Eğer Saldırı kartıysa
            if(card.CardT1 == CardType1.AttackCard)
            {
                card.attack = (int)EditorGUILayout.IntField("Attack", card.attack);
                card.mana = (int)EditorGUILayout.IntField("Mana", card.mana);
            }

            //Eğer Savunma kartıysa
            if (card.CardT1 == CardType1.DefenceCard)
            {
                card.defence = (int)EditorGUILayout.IntField("Defence", card.defence);
                card.mana = (int)EditorGUILayout.IntField("Mana", card.mana);
            }

            //Eğer Enerji kartıysa
            if (card.CardT1 == CardType1.EnergyCard)
            {
                card.mana = (int)EditorGUILayout.IntField("Mana", card.mana);
            }

            //Kart için 2.özelliği görünür kılma fonksiyonu.
            card.CardT2 = (CardType2)EditorGUILayout.EnumPopup("Second Card Type", card.CardT2);

            //Eğer ikinci özellik olarak buff seçildiyse.
            if(card.CardT2 == CardType2.BuffCard)
            {
                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardBuff = (buffs)EditorGUILayout.EnumPopup("Buffs", card.cardBuff);
                card.buffCoefficient = EditorGUILayout.FloatField("Buff Coefficient", card.buffCoefficient);
            }
            //Eğer ikinci özellik olarak debuff seçildiyse.
            else if (card.CardT2 == CardType2.DebuffCard)
            {
                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardDebuff = (debuffs)EditorGUILayout.EnumPopup("Debuffs", card.cardDebuff);
                card.debuffCoefficient = EditorGUILayout.FloatField("Debuff Coefficient", card.debuffCoefficient);
            }
            //Eğer ikinci özellik olarak combine seçildiyse.
            else if (card.CardT2 == CardType2.CombineCard)
            {
                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardCombine = (CombineType)EditorGUILayout.EnumPopup("Combine Card", card.cardCombine);
            }


        }
    }
}
#endif