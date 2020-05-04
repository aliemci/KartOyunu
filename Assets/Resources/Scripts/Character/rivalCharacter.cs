using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Düşmanın hareketleri
[System.Serializable]
public class fightPattern
{
    public move moves;

    [HideInInspector]
    public buffs buff;
    [HideInInspector]
    public debuffs debuff;
}

public enum move
{
    Attack,
    Shield,
    Charge,
    Buff,
    Debuff
}



[CreateAssetMenu(fileName = "New Rival Character", menuName = "Rival Character")]
public class rivalCharacter : Character
{
    [Header("Rival Specific")]
    public bool isRival = true;

    [Header("Rival General Attributes")]
    public int damage;
    public int regain_health;
    public int regain_shield;
    public List<fightPattern> enemyPattern = new List<fightPattern>();

    /*
    [HideInInspector]
    public buffs enemy_buff;
    [HideInInspector]
    public debuffs enemy_debuff;
    */

    [HideInInspector]
    public int movement_index = 0;


    //---------------
    public void do_move(playerCharacter player)
    {

        switch (enemyPattern[movement_index].moves)
        {
            case move.Attack:
                player.takeDamage(damage);
                break;

            case move.Buff:
                doBuff(enemyPattern[movement_index].buff, 1);
                break;

            case move.Charge:
                //EKSİK
                break;

            case move.Debuff:
                player.doDebuff(enemyPattern[movement_index].debuff, 1);
                break;

            case move.Shield:
                shieldApply(regain_shield);
                break;
        }

        //Hareket sayacı bir ilerletiliyor.
        movement_index++;

        //Liste sonsuz olarak dönsün diye modu alınıyor
        movement_index = movement_index % enemyPattern.Count;
    }


}

/*
#if UNITY_EDITOR
[CustomEditor(typeof(rivalCharacter))]
public class rivalCharacter_Editor : Editor
{
    //Düzenleme alanındaki her eylemden sonra bu fonksiyon çağırılıyor.
    public override void OnInspectorGUI()
    {
        //Öntanımlı çizilenler için.
        DrawDefaultInspector();

        //Üzerinde değişiklik yaptığımız nesnenin kart olduğunu belirterek değişkene atıyoruz.
        rivalCharacter rival_character = (rivalCharacter)target;

        //Eğer bool doğruysa buff seçilebilsin.
        if(rival_character.enemyPattern[0].moves == move.Buff)
        {
            //rival_character.enemy_buff = (buffs)EditorGUILayout.EnumPopup("Buffs", rival_character.enemy_buff);
            rival_character.enemyPattern[0].buff = (buffs)EditorGUILayout.EnumPopup("Buffs", rival_character.enemyPattern[rival_character.movement_index].buff);
        }

        //Eğer bool doğruysa debuff seçilebilsin.
        if (rival_character.enemyPattern[0].moves == move.Debuff)
            rival_character.enemyPattern[0].debuff = (debuffs)EditorGUILayout.EnumPopup("Debuffs", rival_character.enemyPattern[rival_character.movement_index].debuff);

    }

}
#endif
*/
