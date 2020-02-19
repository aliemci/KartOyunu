using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class CardDisplay : MonoBehaviour
{
    public Card card;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI shieldText;

    public bool isPlayerOwn;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        nameText.text = card.id;
        descriptionText.text = card.Description;
        manaText.text = card.Mana.ToString();
        attackText.text = card.Attack.ToString();
        shieldText.text = card.Shield.ToString();
        isPlayerOwn = card.isPlayerOwn;
    }
        
}
