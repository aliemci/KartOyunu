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
    public TextMeshProUGUI descriptionText;
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
    }

    void Start()
    {
        Refresh();
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
    }

    public void Refresh()
    {
        nameText.text = card.CardIdentity;
        descriptionText.text = card.Description;
        manaText.text = card.Mana.ToString();
        attackText.text = card.Attack.ToString();
        shieldText.text = card.Shield.ToString();
        isPlayerOwn = card.isPlayerOwn;
    }
        
}
