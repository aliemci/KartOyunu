using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu_Back : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
