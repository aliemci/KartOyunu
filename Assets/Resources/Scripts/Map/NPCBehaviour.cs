using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public enum NPCTypes
    {
        market,
        rival,
        boss
    }

    public NPCTypes NPCType;

    private void OnValidate()
    {
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

    void market()
    {

    }

    void rival()
    {

    }

    void boss()
    {

    }

}
