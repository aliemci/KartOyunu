using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class fight_buttons : MonoBehaviour
{
    
    public void inventory()
    {
        SceneManager.LoadScene("inventory");
    }

    public void card_pile()
    {
        SceneManager.LoadScene("card_pile");
    }
}
