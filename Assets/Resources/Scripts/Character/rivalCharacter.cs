using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SubjectNerd.Utilities;

//Düşmanın hareketleri
[System.Serializable]
public class fightPattern
{
    public move moves;
    
    public buffs buff;
    public int buffCoef;

    public debuffs debuff;
    public int debuffCoef;
}

public enum move
{
    Attack,
    Shield,
    Heal,
    Charge,
    Buff,
    Debuff,
    Special
}



[CreateAssetMenu(fileName = "New Rival Character", menuName = "Rival Character")]
public class rivalCharacter : Character
{
    //--------------- DEĞİŞKENLER ---------------
        
    [HideInInspector]
    public int damage;

    [HideInInspector]
    public int regainHealth;
    [HideInInspector]
    public int regainShield;


    [HideInInspector]
    public List<fightPattern> enemyPattern = new List<fightPattern>();

    [HideInInspector]
    public int movement_index = 0;


    //--------------- FONKSİYONLAR ---------------
    public void do_move(playerCharacter player, GameObject[] rivals)
    {
        //Değişken
        rivalCharacter rival = null;
        int rival_index=0;

        //Eğer hiçbir hareketi yoksa
        if (enemyPattern.Count == 0)
            return;

        
        if (is_confused)
        {
            //Rastgele bir sayı tutuluyor.
            rival_index = Mathf.FloorToInt(Random.Range(0f, rivals.Length));
            //O indisteki düşman alınıyor. Bu Confuse mekaniği için gerekli
            rival = rivals[rival_index].GetComponent<CharacterDisplay>().character as rivalCharacter;
            
            rival.prepareChances();
        }

        Debug.Log(enemyPattern[movement_index].moves);

        this.prepareChances();
        player.prepareChances();

        GameObject playerGO = GameObject.Find("Player");
        
        switch (enemyPattern[movement_index].moves)
        {
            case move.Attack:
                //Stun veya blind halindeyse
                if (this.is_stunned || this.is_blinded)
                    break;

                //Confused halindeyse
                if (is_confused)
                {
                    //Eğer saldıracağı arkadaşı kaçabildiyse
                    if (rival.is_evaded)
                        break;

                    DamageIndicator.CreateDamageIndicator(rivals[rival_index].transform.position, damage * attack_multiplier + attack_factor);

                    //Kendi takımından birine vuruyor.
                    rival.takeDamage(Mathf.Abs(damage * attack_multiplier + attack_factor));
                    attack_multiplier = 1;
                }
                else
                {
                    //Eğer oyuncu kaçabildiyse
                    if (player.is_evaded)
                        break;

                    DamageIndicator.CreateDamageIndicator(playerGO.transform.position, damage * attack_multiplier + attack_factor);

                    //Oyuncuya vuruyor.
                    player.takeDamage(Mathf.Abs(damage * attack_multiplier + attack_factor));
                    attack_multiplier = 1;
                }

                break;

            //---------------

            case move.Shield:
                if (is_stunned)
                    break;

                shieldApply(regainShield);

                break;

            //---------------

            case move.Heal:
                if (is_stunned)
                    break;

                healApply(regainHealth);

                break;

            //---------------

            case move.Charge:
                if (is_stunned)
                    break;

                //EKSİK
                break;

            //---------------

            case move.Buff:
                //Debug.Log(name + " buffed by " + enemyPattern[movement_index].buff);
                if (enemyPattern[movement_index].buff != buffs.None)
                {
                    buffQueue bq = new buffQueue();
                    bq.buff = enemyPattern[movement_index].buff;
                    bq.coefficient = enemyPattern[movement_index].buffCoef;

                    buffList.Add(bq);
                }
                    //doBuff(enemyPattern[movement_index].buff, enemyPattern[movement_index].buffCoef);
                break;

            //---------------

            case move.Debuff:
                //Debug.Log(player + " debuffed by " + enemyPattern[movement_index].debuff);
                if (enemyPattern[movement_index].debuff != debuffs.None)
                {
                    debuffQueue dq = new debuffQueue();
                    dq.debuff = enemyPattern[movement_index].debuff;
                    dq.coefficient = enemyPattern[movement_index].debuffCoef;

                    player.debuffList.Add(dq);
                }
                //player.doDebuff(enemyPattern[movement_index].debuff, enemyPattern[movement_index].debuffCoef);
                break;

            //---------------

            case move.Special:
                if (is_stunned)
                    break;

                //EKSİK
                break;
        }

        //Hareket sayacı bir ilerletiliyor.
        movement_index++;

        //Liste sonsuz olarak dönsün diye modu alınıyor
        movement_index = movement_index % enemyPattern.Count;
    }


}


#if UNITY_EDITOR
[CustomEditor(typeof(rivalCharacter))]
public class rivalCharacter_Editor : Editor
{
    enum displayFieldType { DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields }
    displayFieldType DisplayFieldType;

    rivalCharacter riv;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;

    void OnEnable()
    {
        riv = (rivalCharacter)target;
        GetTarget = new SerializedObject(riv);
        ThisList = GetTarget.FindProperty("enemyPattern");
        
    }

    //Düzenleme alanındaki her eylemden sonra bu fonksiyon çağırılıyor.
    public override void OnInspectorGUI()
    {
        //Öntanımlı çizilenler için.
        DrawDefaultInspector();

        GetTarget.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Basic AI", EditorStyles.boldLabel);

        ListSize = ThisList.arraySize;

        if (ListSize != ThisList.arraySize)
        {
            while (ListSize > ThisList.arraySize)
            {
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            }
            while (ListSize < ThisList.arraySize)
            {
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            //Atamalar
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty moves = MyListRef.FindPropertyRelative("moves");
            SerializedProperty buff = MyListRef.FindPropertyRelative("buff");
            SerializedProperty buffCoef = MyListRef.FindPropertyRelative("buffCoef");
            SerializedProperty debuff = MyListRef.FindPropertyRelative("debuff");
            SerializedProperty debuffCoef = MyListRef.FindPropertyRelative("debuffCoef");

            //-------------------------------------------------------
            //Yanyana olsun diye gruplandırıyor.
            Rect r = EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(i + 1 + ". Move", EditorStyles.boldLabel);

            //Sağ üst köşesine silme tuşu ekliyor
            if (GUILayout.Button("Delete Move"))
            {
                riv.enemyPattern.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
            //-------------------------------------------------------

            //-------------------------------------------------------

            EditorGUILayout.PropertyField(moves);

            //Buff ise
            if(moves.enumDisplayNames[moves.enumValueIndex] == move.Buff.ToString())
            {
                EditorGUILayout.PropertyField(buff);
                EditorGUILayout.PropertyField(buffCoef);
            }
            //Debuff ise
            else if (moves.enumDisplayNames[moves.enumValueIndex] == move.Debuff.ToString())
            {
                EditorGUILayout.PropertyField(debuff);
                EditorGUILayout.PropertyField(debuffCoef);
            }
            //Shield ise
            else if (moves.enumDisplayNames[moves.enumValueIndex] == move.Shield.ToString())
            {
                EditorGUILayout.PropertyField(GetTarget.FindProperty("regainShield"));
            }
            //Heal ise
            else if (moves.enumDisplayNames[moves.enumValueIndex] == move.Heal.ToString())
            {
                EditorGUILayout.PropertyField(GetTarget.FindProperty("regainHealth"));
            }
            //Special ise
            else if (moves.enumDisplayNames[moves.enumValueIndex] == move.Attack.ToString())
            {
                EditorGUILayout.PropertyField(GetTarget.FindProperty("damage"));
            }
            //-------------------------------------------------------

            //-------------------------------------------------------

            //Her bir hareketin altına yatay bir doğru parçası çiziyor.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            //-------------------------------------------------------

        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //-------------------------------------------------------

        //Sonuna ekleme tuşu ekliyor
        if (GUILayout.Button("Add New Move"))
        {
            riv.enemyPattern.Add(new fightPattern());
        }
        //-------------------------------------------------------

        GetTarget.ApplyModifiedProperties();
        
    }
}
#endif

