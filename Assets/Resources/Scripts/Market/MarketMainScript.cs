using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MarketMainScript : MonoBehaviour
{
    

    public void backToMap()
    {
        //Haritayı yüklüyor.
        SceneManager.LoadScene(1);
    }


}
