using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

public enum NPCTypes
    {
        market,
        rival,
        boss
    }

public class NPCBehaviour : MonoBehaviour, IPointerClickHandler
{

    public NPCTypes NPCType;

    public hexagon parentHex;

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }


    void market()
    {
        //Market sahnesi yükleniyor.
        SceneManager.LoadScene(5);
    }

    void rival(GameObject rivalObj)
    {
        Destroy(rivalObj);

        Debug.Log("Rival Ögesi Silindi!");
        //Dövüş Sahnesini yüklüyor.
        SceneManager.LoadScene(2);
    }

    void boss(GameObject rivalObj)
    {
        Destroy(rivalObj);
        Debug.Log("Boss Ögesi Silindi!");

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!player.GetComponent<PlayerMovement>().is_camera_dragged)
        {
            Debug.Log("Name:" + this.gameObject.name);
            if (player.GetComponent<PlayerMovement>().go_to(this.parentHex.hexObj))
            {
                switch (NPCType)
                {
                    case NPCTypes.market:
                        market();
                        break;

                    case NPCTypes.rival:
                        rival(this.gameObject);
                        break;

                    case NPCTypes.boss:
                        boss(this.gameObject);
                        break;
                }
            }
        }
    }
}
