using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCBehaviour : MonoBehaviour, IPointerClickHandler
{
    public enum NPCTypes
    {
        market,
        rival,
        boss
    }

    public NPCTypes NPCType;

    public hexagon parentHex;

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }


    void market()
    {

    }

    void rival()
    {
        SceneManager.LoadScene(0);
    }

    void boss()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!player.GetComponent<PlayerMovement>().is_camera_dragged)
        {
            Debug.Log("Name:" + this.gameObject.name);
            player.GetComponent<PlayerMovement>().go_to(this.parentHex.hexObj);
            switch (NPCType)
            {
                case NPCTypes.market:
                    market();
                    break;

                case NPCTypes.rival:
                    rival();
                    break;

                case NPCTypes.boss:
                    boss();
                    break;
            }
        }
    }
}
