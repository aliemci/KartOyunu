using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum debuffs
{
    Poison,
    Confused,
    Stun,
    Blind,
    Frailness,
    Weakness,
    Tired
}

public class DebuffCard : MonoBehaviour
{
    public debuffs debuff;

    public void debuffApplier()
    {
        switch (debuff)
        {
            case debuffs.Poison:
                poison();
                break;

            case debuffs.Confused:

                break;

            case debuffs.Stun:

                break;

            case debuffs.Blind:

                break;

            case debuffs.Frailness:

                break;

            case debuffs.Weakness:

                break;

            case debuffs.Tired:

                break;
        }
    }

    private void poison( )
    {
        
    }
}
