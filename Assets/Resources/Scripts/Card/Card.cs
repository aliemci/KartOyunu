using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

// Enumlar
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

public enum AttackRegime
{
    Point,
    Horizontal,
    Vertical,
    Triangle,
    AllRegions
}
// --------------


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
    public int buffCoefficient;
    [HideInInspector]
    public int debuffCoefficient;

    [HideInInspector]
    public int attack, defence, mana;

    [HideInInspector]
    public bool isPlayerOwn, isCardUsed;


    [Header("Attack Specs")][HideInInspector]
    //public List<int> attack_range = new List<int>() { 0,0,0,0 };
    public int[] attack_range = new int[4];
    [HideInInspector]
    public bool[] attackable_range = new bool[4];
    [HideInInspector]
    public AttackRegime attackRegime;
    
    //Kontrol
    void OnValidate()
    {
        cardName = this.name;

        if(attackRegime == AttackRegime.Triangle)
        {
            //Debug.Log("Triangle!");
            int howManyAreasSelected=0;
            foreach (bool item in attackable_range)
            {
                if (item == true)
                {
                    howManyAreasSelected++;
                    //Debug.Log("true");
                }
            }
            if (howManyAreasSelected != 3)
            {
                //Debug.LogWarning("Triangle seçili olduğu zaman 3 tane yer seçilmelidir!");
                attackable_range[0] = true;
                attackable_range[1] = true;
                attackable_range[2] = true;
            }
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

            if (show)
            {
                if(attackable_range[i] == true)
                {
                    // Karakterin saydamlığı ayarlıyor.
                    rival.GetComponent<SpriteRenderer>().material.color =
                        new Color(
                        rival.GetComponent<SpriteRenderer>().material.color.r,
                        rival.GetComponent<SpriteRenderer>().material.color.g,
                        rival.GetComponent<SpriteRenderer>().material.color.b,
                        1f
                        );

                    //Eğer karakter vuruşdan etkilenmiyorsa onun collider aracını kapatıyor.
                    rival.GetComponent<BoxCollider2D>().enabled = true;
                }
                else
                {
                    // Karakterin saydamlığı ayarlıyor.
                    rival.GetComponent<SpriteRenderer>().material.color =
                        new Color(
                        rival.GetComponent<SpriteRenderer>().material.color.r,
                        rival.GetComponent<SpriteRenderer>().material.color.g,
                        rival.GetComponent<SpriteRenderer>().material.color.b,
                        0.1f
                        );

                    //Eğer karakter vuruşdan etkilenmiyorsa onun collider aracını kapatıyor.
                    rival.GetComponent<BoxCollider2D>().enabled = false;
                }

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
        EditorGUILayout.LabelField("Main Type", label);
        //İlk kart tipini görünür kılıyor.
        card.CardT1 = (CardType1)EditorGUILayout.EnumPopup("First Card Type", card.CardT1);
        //Saldırı Tipini belirliyor.
        EditorGUILayout.LabelField("Attack Regime", label);
        card.attackRegime = (AttackRegime)EditorGUILayout.EnumPopup("Regime Type", card.attackRegime);

        //Büyüklüğün belirlenmesi
        switch (card.attackRegime)
        {
            case AttackRegime.Point:
                //card.attack_range = new List<int>(new int[1]);
                EditorGUILayout.LabelField("Attackable Range", label); // Boşluk
                card.attackable_range[0] = EditorGUILayout.Toggle("Upper Left", card.attackable_range[0]);
                card.attackable_range[1] = EditorGUILayout.Toggle("Upper Right", card.attackable_range[1]);
                card.attackable_range[2] = EditorGUILayout.Toggle("Lower Left", card.attackable_range[2]);
                card.attackable_range[3] = EditorGUILayout.Toggle("Lower Right", card.attackable_range[3]);
                //Debug.Log(card.attack_range[0]);
                break;
            case AttackRegime.Horizontal:
                //card.attack_range = new List<int>(new int[2]);
                EditorGUILayout.LabelField("Attackable Range", label); // Boşluk
                card.attackable_range[0] = EditorGUILayout.Toggle("Upper", card.attackable_range[0]);
                card.attackable_range[1] = card.attackable_range[0];
                card.attackable_range[2] = EditorGUILayout.Toggle("Lower", card.attackable_range[2]);
                card.attackable_range[3] = card.attackable_range[2];
                //Debug.Log(card.attack_range);
                break;
            case AttackRegime.Vertical:
                //card.attack_range = new List<int>(new int[2]);
                EditorGUILayout.LabelField("Attackable Range", label); // Boşluk
                card.attackable_range[0] = EditorGUILayout.Toggle("Left", card.attackable_range[0]);
                card.attackable_range[1] = EditorGUILayout.Toggle("Right", card.attackable_range[1]);
                card.attackable_range[2] = card.attackable_range[0];
                card.attackable_range[3] = card.attackable_range[1];
                //Debug.Log(card.attack_range);
                break;
            case AttackRegime.Triangle:
                //card.attack_range = new List<int>(new int[3]);
                EditorGUILayout.LabelField("Attackable Range", label); // Boşluk
                card.attackable_range[0] = EditorGUILayout.Toggle("Upper Left", card.attackable_range[0]);
                card.attackable_range[1] = EditorGUILayout.Toggle("Upper Right", card.attackable_range[1]);
                card.attackable_range[2] = EditorGUILayout.Toggle("Lower Left", card.attackable_range[2]);
                card.attackable_range[3] = EditorGUILayout.Toggle("Lower Right", card.attackable_range[3]);
                //Debug.Log(card.attack_range);
                break;
            case AttackRegime.AllRegions:
                //card.attack_range = new List<int>(new int[4]);
                card.attackable_range[0] = true;
                card.attackable_range[1] = true;
                card.attackable_range[2] = true;
                card.attackable_range[3] = true;
                //Debug.Log(card.attack_range);
                break;
        }


        //Eğer ilk kart özelliği buff&debuff değilse, ikinci özelliği görünür kılıyoruz.
        if (card.CardT1 != CardType1.BuffCard && card.CardT1 != CardType1.DebuffCard)
        {
            //Eğer Saldırı kartıysa
            if(card.CardT1 == CardType1.AttackCard)
            {
                //EditorGUILayout.LabelField("", label); // Boşluk
                EditorGUILayout.LabelField("Attack Specs", label);
                //card.attack = (int)EditorGUILayout.IntField("Attack", card.attack);
                card.mana = (int)EditorGUILayout.IntField("Mana", card.mana);

                EditorGUILayout.LabelField("Attack(s) Value(s)", label); // Boşluk

                switch (card.attackRegime)
                {
                    case AttackRegime.Point:
                        card.attack_range[0] = (int)EditorGUILayout.FloatField("Center", card.attack_range[0]);
                        break;
                    case AttackRegime.Horizontal:
                        card.attack_range[0] = (int)EditorGUILayout.FloatField("Left", card.attack_range[0]);
                        card.attack_range[1] = (int)EditorGUILayout.FloatField("Right", card.attack_range[1]);
                        break;
                    case AttackRegime.Vertical:
                        card.attack_range[0] = (int)EditorGUILayout.FloatField("Upper", card.attack_range[0]);
                        card.attack_range[1] = (int)EditorGUILayout.FloatField("Lower", card.attack_range[1]);
                        break;
                    case AttackRegime.Triangle:
                        card.attack_range[0] = (int)EditorGUILayout.FloatField("Center", card.attack_range[0]);
                        card.attack_range[1] = (int)EditorGUILayout.FloatField("Horizontal", card.attack_range[1]);
                        card.attack_range[2] = (int)EditorGUILayout.FloatField("Vertical", card.attack_range[2]);
                        break;
                    case AttackRegime.AllRegions:
                        card.attack_range[0] = (int)EditorGUILayout.FloatField("Upper Left", card.attack_range[0]);
                        card.attack_range[1] = (int)EditorGUILayout.FloatField("Upper Right", card.attack_range[1]);
                        card.attack_range[2] = (int)EditorGUILayout.FloatField("Lower Left", card.attack_range[2]);
                        card.attack_range[3] = (int)EditorGUILayout.FloatField("Lower Right", card.attack_range[3]);
                        break;
                }

                //EditorGUILayout.LabelField("Attackable Range", label); // Boşluk
                //card.attackable_range[0] = EditorGUILayout.Toggle("Upper Left", card.attackable_range[0]);
                //card.attackable_range[1] = EditorGUILayout.Toggle("Upper Right", card.attackable_range[1]);
                //card.attackable_range[2] = EditorGUILayout.Toggle("Lower Left", card.attackable_range[2]);
                //card.attackable_range[3] = EditorGUILayout.Toggle("Lower Right", card.attackable_range[3]);

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
                card.buffCoefficient = EditorGUILayout.IntField("Buff Coefficient", card.buffCoefficient);
            }

            //Eğer ikinci özellik olarak debuff seçildiyse.
            else if (card.CardT2 == CardType2.DebuffCard)
            {
                EditorGUILayout.LabelField("Debuff Type", label);
                //Kartın bilgilerini girebilmesi için gerekli alanları görünür kılıyor.
                card.cardDebuff = (debuffs)EditorGUILayout.EnumPopup("Debuffs", card.cardDebuff);
                card.debuffCoefficient = EditorGUILayout.IntField("Debuff Coefficient", card.debuffCoefficient);
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
                        card.buffCoefficient = EditorGUILayout.IntField(new GUIContent("Buff Coefficient", "Girilen sayı kadar etki yapacak."), card.buffCoefficient);
                        break;

                    //Bunda ise ihtimal cinsinden katsayı gerekiyor.
                    case buffs.Alertness:
                        card.buffCoefficient = EditorGUILayout.IntField(new GUIContent("Buff Coefficient", "Girilen sayı üzerinden ihtimal hesaplanacak."), card.buffCoefficient);
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
                        card.debuffCoefficient = EditorGUILayout.IntField(new GUIContent("Debuff Coefficient", "Girilen sayı kadar can gidecek."), card.debuffCoefficient);
                        break;

                    case debuffs.Confused:
                    case debuffs.Blind:
                        card.debuffCoefficient = EditorGUILayout.IntField(new GUIContent("Debuff Coefficient", "Girilen sayı üzerinden ihtimal hesaplanacak."), card.debuffCoefficient);
                        break;

                    case debuffs.Frailness:
                    case debuffs.Tired:
                    case debuffs.Weakness:
                        card.debuffCoefficient = EditorGUILayout.IntField(new GUIContent("Debuff Coefficient", "Girilen sayı kadar etki yapacak."), card.debuffCoefficient);
                        break;
                }
            }
        }

    }
}
#endif