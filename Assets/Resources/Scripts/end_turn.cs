using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_turn : MonoBehaviour
{
    
    public void next_turn()
    {
        enemy_turn();
    }

    private void enemy_turn()
    {
        Transform enemy_parent = GameObject.Find("Enemy-deck").transform;
        int childCount = enemy_parent.childCount;
        for(int i=0; i<childCount; i++)
        {
            Transform enemy = enemy_parent.GetChild(i);
            enemy_movement(enemy);
        }
    }

    private void enemy_movement(Transform enemy)
    {
        Debug.Log(enemy.name);

        //Kötü bir kod:
        //Düşmanlara gidip onların "EnemyDisplay" koduna erişiyor.
        //O kodda olan "Enemy" scriptable object sınıfına erişiyor.
        //"Enemy" sınıfında olan "moveFunction" fonksiyonunu çalıştırıyor.
        //İşlevin içine aldığı parametre de yine "Enemy" sınıfının içindeki veri tipi.
        enemy.GetComponent<EnemyDisplay>().enemy.moveFunction(enemy.GetComponent<EnemyDisplay>().enemy.moves);
    }
}
