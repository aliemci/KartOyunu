using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyGenerator : MonoBehaviour
{
    Enemy[] EnemyTypes;

    public GameObject Prefab;




    void Awake()
    {
        //Enemy_Types klasörünün altındaki bütün Enemy tipindeki nesneleri alıyor.
        EnemyTypes = Resources.LoadAll<Enemy>("EnemyTypes");

        //Boş bir liste oluşturuluyor.
        List<Enemy> Enemies = new List<Enemy>();

        for(int x=0; x<3; x++)
        {
            //Rastgele bir tamsayı alıyor.
            int index = Random.Range(0, EnemyTypes.Length);

            //Herhangi bir kart tipi rastgele desteye ekleniyor.
            Enemies.Add(EnemyTypes[index]);
        
            //Öncelikle prefab ile Gameobject oluşturuluyor.
            GameObject createdCard = Instantiate(Prefab);

            //Oluşturulan Gameobject içindeki EnemyDisplay koduna erişiyor.
            //Koddaki card değişkenine elimizdeki enemy tipini atıyor.
            createdCard.GetComponent<EnemyDisplay>().enemy = EnemyTypes[index];

            //Layer olarak 8(Düşman) atanıyor.
            createdCard.layer = 8;

            //Yeni oluşturulmuş OyunNesnesi'nin ebeveyni olarak "Enemy-deck"i veriyor.
            //Bu sayede Deck içine geçip sıralanıyor.
            createdCard.transform.SetParent(GameObject.Find("Enemy-deck").transform);
        }


    }
        
}
