using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum buffs
{
    Adrenaline,
    Alertness,
    Castle,
    Economiser,
    Puffed,
    Resistance,
    Invincible
}

public class BuffCard : MonoBehaviour
{
    public buffs buff;

    public void buffApplier(Character effectedCharacter)
    {
        switch (buff)
        {
            case buffs.Adrenaline:
                adrenaline(effectedCharacter);
                break;

            case buffs.Alertness:

                break;

            case buffs.Castle:

                break;

            case buffs.Economiser:

                break;

            case buffs.Puffed:

                break;

            case buffs.Resistance:

                break;

            case buffs.Invincible:

                break;
        }
    }

    private void adrenaline(Character subject)
    {
        subject.attack_factor = 2;
    }
}
