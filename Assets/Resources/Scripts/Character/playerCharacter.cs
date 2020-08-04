using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
//Başlangıçta karakterin sahip olacağı kartlar
public class starterCards
{
    public Card card;
    public int count;
}


[CreateAssetMenu(fileName = "New Player Character", menuName = "Player Character")]
public class playerCharacter : Character
{
    [Header("Player Specific")]
    public bool isUsing = false;
    public List<starterCards> StartingCards = new List<starterCards>();


    
    public void add_card_to_player(Card card)
    {
        //Kart eğer destede varsa sayısını arttırıyor.
        foreach(starterCards sCard in StartingCards)
        {
            if(sCard.card == card)
            {
                sCard.count++;
                return;
            }
        }
        //Eğer destede yoksa yeni oluşturuyor.
        starterCards temp = new starterCards();
        temp.card = card;
        temp.count = 1;

        StartingCards.Add(temp);
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(playerCharacter))]
public class playerCharacter_Editor : Editor
{
    //Düzenleme alanındaki her eylemden sonra bu fonksiyon çağırılıyor.
    public override void OnInspectorGUI()
    {
        //Öntanımlı  çizilenler için.
        DrawDefaultInspector();

        //Üzerinde değişiklik yaptığımız nesnenin kart olduğunu belirterek değişkene atıyoruz.
        playerCharacter player_character = (playerCharacter)target;


    }

}
#endif
