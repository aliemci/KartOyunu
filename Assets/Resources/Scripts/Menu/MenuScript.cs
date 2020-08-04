using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject MainMenu, CharacterSelection;

    public void new_game()
    {
        character_selection_screen();
    }

    void character_selection_screen()
    {
        CharacterSelection.SetActive(true);
        MainMenu.SetActive(false);

        playerCharacter[] playableCharacters = Resources.LoadAll("Players") as playerCharacter[];

    }

    public void return_to_main_menu()
    {
        CharacterSelection.SetActive(false);
        MainMenu.SetActive(true);

    }

    public void select()
    {
        PlayerData.player = Instantiate(GameObject.Find("SelectedCharacter").GetComponent<CharacterHolderForMenu>().selectedChar);
        
        if(PlayerData.player != null)
            SaveSystem.save_player(PlayerData.player);

        // Haritaya geçiyor.
        SceneManager.LoadScene(2);

    }

}
