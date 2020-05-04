using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//Başlangıçta karakterin sahip olacağı kartlar
[System.Serializable]
public class starterCards
{
    public Card card;
    public int count;
}



[CreateAssetMenu(fileName = "New Player Character", menuName = "Player Character")]
public class playerCharacter : Character
{
    [Header("Player Specific")]
    public bool isPlayer = true;
    public List<starterCards> StartingCards = new List<starterCards>();

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
