using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class CardDisplay : MonoBehaviour
{

    //Public Variables:
    public Card card;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI shieldText;

    public bool isPlayerOwn;

    //Private Variables:
    private Vector2 resolution_scale;


    //--------END OF VARIABLES--------

    void Awake()
    {
        resolution_scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;

        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        attackText = transform.Find("Attack").GetComponent<TextMeshProUGUI>();
        shieldText = transform.Find("Shield").GetComponent<TextMeshProUGUI>();
        manaText = transform.Find("Mana").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        Refresh();
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
    }

    public void Refresh()
    {
        nameText.text = card.cardName;
        attackText.text = card.attack.ToString();
        shieldText.text = card.defence.ToString();
        manaText.text = card.mana.ToString();
        isPlayerOwn = card.isPlayerOwn;
    }

}
