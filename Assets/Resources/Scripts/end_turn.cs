using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_turn : MonoBehaviour
{
    
    public void next_turn()
    {
        Debug.Log("Next Turn!");
        //Oyuncu için fonksiyonlar: Önce sıradaki tur fonksiyonuyla listedeki buff debufflar etkiyecek.
        //Sonra bu değişiklikler yazdırılacak.
        GameObject.Find("Player").GetComponent<CharacterDisplay>().character.next_turn();
        GameObject.Find("Player").GetComponent<CharacterDisplay>().healthManaWriter();

        //Aynı şey her düşman için de kontrol edilecek.
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            obj.GetComponent<CharacterDisplay>().character.next_turn();
            obj.GetComponent<CharacterDisplay>().checkIsDead();
        }

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
        //enemy.GetComponent<EnemyDisplay>().enemy.moveFunction(enemy.GetComponent<EnemyDisplay>().enemy.moves);
    }
}
