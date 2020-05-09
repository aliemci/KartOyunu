using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Düşmanın hareketleri
[System.Serializable]
public class fightPattern
{
    public move moves;

    //[HideInInspector]
    public buffs buff;
    //[HideInInspector]
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

    [Header("Basic AI")]
    public List<fightPattern> enemyPattern = new List<fightPattern>();


    [HideInInspector]
    public int movement_index = 0;


    //---------------
    public void do_move(playerCharacter player)
    {
        //Eğer hiçbir hareketi yoksa
        if (enemyPattern.Count == 0)
            return;

        switch (enemyPattern[movement_index].moves)
        {
            case move.Attack:
                player.takeDamage(damage);
                break;

            case move.Buff:
                Debug.Log(name + " buffed by " + enemyPattern[movement_index].buff);
                if (enemyPattern[movement_index].buff != buffs.None)
                    doBuff(enemyPattern[movement_index].buff, 50);
                break;

            case move.Charge:
                //EKSİK
                break;

            case move.Debuff:
                Debug.Log(player + " debuffed by " + enemyPattern[movement_index].debuff);
                if (enemyPattern[movement_index].debuff != debuffs.None)
                    player.doDebuff(enemyPattern[movement_index].debuff, 5);
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

        foreach(fightPattern fp in rival_character.enemyPattern)
        {
            //Eğer bool doğruysa buff seçilebilsin.
            if(fp.moves == move.Buff)
            {
                //Debuff seçeneğini eliyor.
                fp.debuff = debuffs.None;

                //Buff seçeneğini boş seçmesini engelliyor.
                if (fp.buff == buffs.None)
                    fp.buff = buffs.Adrenaline;
            }

            //Eğer bool doğruysa debuff seçilebilsin.
            if (fp.moves == move.Debuff)
            {
                fp.buff = buffs.None;
                
                if (fp.debuff == debuffs.None)
                    fp.debuff = debuffs.Blind;
            }
        }
        
    }
}
#endif

