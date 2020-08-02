using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterShowScript : MonoBehaviour
{
    public playerCharacter playerChar;
    public GameObject text;

    private void OnValidate()
    {
        if(playerChar != null)
            GetComponent<SpriteRenderer>().sprite = playerChar.CharacterSprite;
        else
            GetComponent<SpriteRenderer>().sprite = null;
    }

    private void OnMouseDown()
    {
        if (text.GetComponent<TextMeshProUGUI>().text != playerChar.name)
        {
            text.GetComponent<TextMeshProUGUI>().text = playerChar.name;
            GameObject.Find("Select").GetComponent<Button>().interactable = true;
        }
    }
}
