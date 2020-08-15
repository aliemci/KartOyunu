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

    [Header("Specifications")]
    public string cardName;
    
    [HideInInspector]
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

    [HideInInspector]
    public bool isPlayerOwn, isCardUsed;

    private const int attack_range_size = 4;
    [Header("Attack Specs")][HideInInspector]
    public float[] attack_range = new float[attack_range_size];

    
    //Kontrol
    void OnValidate()
    {
        if (attack_range.Length != attack_range_size)
        {
            Debug.LogWarning("attack_range listesinin boyutuyla oynama!");
            Array.Resize(ref attack_range, attack_range_size);
        }
    }
    

    //Kartların vurabileceği rakipleri şekillendiriyor.
    public void attackable_enemies(bool show)
    {
        //Eğer Combiner açıksa fonksiyondan çıksın
        try
        {
            var combiner = GameObject.Find("CombineCardPanel").GetComponent<CombineWindowMainScript>();
            return;
        }
        catch
        {
            Debug.Log("Combiner açık değil, devam!");
        }

        //Değişken ataması
        GameObject enemy_deck = GameObject.Find("Enemy-deck");

        //Düşmanların hepsine tek tek bakıyor. attack_range listesine göre materyallerinde değişiklik yapıyor.
        for(int i=0; i < enemy_deck.transform.childCount; i++){

            /*
            Debug.Log("Card:" + cardName);
            Debug.Log("Index:" + i);
            */

            //Tanımlama
            GameObject rival;

            //Kapatılabilir. Eğer kartların etkilediği array düzgün şekilde ayarlanırsa.
            try {
                rival = enemy_deck.transform.GetChild(i).gameObject;
            }
            catch
            {
                continue;
            }

            rival = enemy_deck.transform.GetChild(i).gameObject;

            if (!show)
            {
                // Karakterin saydamlığı ayarlıyor.
                rival.GetComponent<SpriteRenderer>().material.color =
                    new Color(
                    rival.GetComponent<SpriteRenderer>().material.color.r,
                    rival.GetComponent<SpriteRenderer>().material.color.g,
                    rival.GetComponent<SpriteRenderer>().material.color.b,
                    attack_range[i] + 0.1f
                    );

                //Eğer karakter vuruşdan etkilenmiyorsa onun collider aracını kapatıyor.
                if(attack_range[i] == 0f)
                    rival.GetComponent<BoxCollider2D>().enabled = false;


            }
            else{
                // Karakterin saydamlığı ayarlıyor.
                rival.GetComponent<SpriteRenderer>().material.color =
                    new Color(
                    rival.GetComponent<SpriteRenderer>().material.color.r,
                    rival.GetComponent<SpriteRenderer>().material.color.g,
                    rival.GetComponent<SpriteRenderer>().material.color.b,
                    1f
                    );

                rival.GetComponent<BoxCollider2D>().enabled = true;
            }

        }

    }

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

        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
        //Üzerinde değişiklik yaptığımız nesnenin kart olduğunu belirterek değişkene atıyoruz.
        Card card = (Card)target;

        //Yazı stili
        GUIStyle label = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold
        };

        //Düz çizgi
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //Başlık
        EditorGUILayout.LabelField("First Type", label);
        //İlk kart tipini görünür kılıyor.
        card.CardT1 = (CardType1)EditorGUILayout.EnumPopup("First Card Type", card.CardT1);


        //Eğer ilk kart özelliği buff&debuff değilse, ikinci özelliği görünür kılıyoruz.
        if (card.CardT1 != CardType1.BuffCard && card.CardT1 != CardType1.DebuffCard)
        {
            //Eğer Saldırı kartıysa
            if(card.CardT1 == CardType1.AttackCard)
            {
                //EditorGUILayout.LabelField("", label); // Boşluk
                EditorGUILayout.LabelField("Attack Specs", label);
                card.attack = (int)EditorGUILayout.IntField("Attack", card.attack);
                card.mana = (int)EditorGUILayout.IntField("Mana", card.mana);

                EditorGUILayout.LabelField("Attack Range", label); // Boşluk
                card.attack_range[0] = (float)EditorGUILayout.FloatField("Upper Left", card.attack_range[0]);
                card.attack_range[1] = (float)EditorGUILayout.FloatField("Upper Right", card.attack_range[1]);
                card.attack_range[2] = (float)EditorGUILayout.FloatField("Lower Left", card.attack_range[2]);
                card.attack_range[3] = (float)EditorGUILayout.FloatField("Lower Right", card.attack_range[3]);

            }

            //Eğer Savunma kartıysa
            else if (card.CardT1 == CardType1.DefenceCard)
            {
                //EditorGUILayout.LabelField("", label); // Boşluk
                EditorGUILayout.LabelField("Defence Specs", label);
                card.defence = (int)EditorGUILayout.IntField("Defence", card.defence);
                card.mana = (int)EditorGUILayout.IntField("Mana", card.mana);
            }

            //Eğer Enerji kartıysa
            else if (card.CardT1 == CardType1.EnergyCard)
            {
                //EditorGUILayout.LabelField("", label); // Boşluk
                EditorGUILayout.LabelField("Energy Specs", label);
                card.mana = (int)EditorGUILayout.IntField("Mana", card.mana);
            }


            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Second Type", label);
            //Kart için 2.özelliği görünür kılma fonksiyonu.
            card.CardT2 = (CardType2)EditorGUILayout.EnumPopup("Second Card Type", card.CardT2);


            //Eğer ikinci özellik olarak buff seçildiyse.
            if(card.CardT2 == CardType2.BuffCard)
            {
                EditorGUILayout.LabelField("Buff Type", label);
                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardBuff = (buffs)EditorGUILayout.EnumPopup("Buffs", card.cardBuff);
                card.buffCoefficient = EditorGUILayout.FloatField("Buff Coefficient", card.buffCoefficient);
            }

            //Eğer ikinci özellik olarak debuff seçildiyse.
            else if (card.CardT2 == CardType2.DebuffCard)
            {
                EditorGUILayout.LabelField("Debuff Type", label);
                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardDebuff = (debuffs)EditorGUILayout.EnumPopup("Debuffs", card.cardDebuff);
                card.debuffCoefficient = EditorGUILayout.FloatField("Debuff Coefficient", card.debuffCoefficient);
            }

            //Eğer ikinci özellik olarak combine seçildiyse.
            else if (card.CardT2 == CardType2.CombineCard)
            {
                EditorGUILayout.LabelField("Combine Type", label);
                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardCombine = (CombineType)EditorGUILayout.EnumPopup("Combine Card", card.cardCombine);
            }


        }

        //Eğer kartlar buff veya debuff ise
        else
        {
            //Eğer birinci özellik olarak buff seçildiyse.
            if (card.CardT1 == CardType1.BuffCard)
            {
                EditorGUILayout.LabelField("Buff Type", label);

                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardBuff = (buffs)EditorGUILayout.EnumPopup("Buffs", card.cardBuff);

                //Kart tipine göre yardımcı yazı yazdırılıyor.
                switch (card.cardBuff)
                {
                    //Bunlarda herhangi bir katsayı vermek gerekmiyor.
                    case buffs.Adrenaline:
                    case buffs.Resistance:
                    case buffs.Invincible:
                        break;

                    //Bunlarda düz katsayı gerekiyor.
                    case buffs.Castle:
                    case buffs.Economiser:
                    case buffs.Regenerate:
                    case buffs.Puffed:
                        card.buffCoefficient = EditorGUILayout.FloatField(new GUIContent("Buff Coefficient", "Girilen sayı kadar etki yapacak."), card.buffCoefficient);
                        break;

                    //Bunda ise ihtimal cinsinden katsayı gerekiyor.
                    case buffs.Alertness:
                        card.buffCoefficient = EditorGUILayout.FloatField(new GUIContent("Buff Coefficient", "Girilen sayı üzerinden ihtimal hesaplanacak."), card.buffCoefficient);
                        break;
                }
            }
            //Eğer birinci özellik olarak debuff seçildiyse.
            else if (card.CardT1 == CardType1.DebuffCard)
            {
                EditorGUILayout.LabelField("Debuff Type", label);

                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardDebuff = (debuffs)EditorGUILayout.EnumPopup("Debuffs", card.cardDebuff);

                //Kart tipine göre yardımcı yazı yazdırılıyor.
                switch (card.cardDebuff)
                {
                    case debuffs.Stun:
                        break;

                    case debuffs.Poison:
                    case debuffs.Burn:
                        card.debuffCoefficient = EditorGUILayout.FloatField(new GUIContent("Debuff Coefficient", "Girilen sayı kadar can gidecek."), card.debuffCoefficient);
                        break;

                    case debuffs.Confused:
                    case debuffs.Blind:
                        card.debuffCoefficient = EditorGUILayout.FloatField(new GUIContent("Debuff Coefficient", "Girilen sayı üzerinden ihtimal hesaplanacak."), card.debuffCoefficient);
                        break;

                    case debuffs.Frailness:
                    case debuffs.Tired:
                    case debuffs.Weakness:
                        card.debuffCoefficient = EditorGUILayout.FloatField(new GUIContent("Debuff Coefficient", "Girilen sayı kadar etki yapacak."), card.debuffCoefficient);
                        break;
                }
            }
        }

    }
}
#endif