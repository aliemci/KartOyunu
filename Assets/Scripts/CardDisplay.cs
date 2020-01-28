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
    public TextMeshProUGUI healthText;

    public bool isPlayerOwn;

    void Start()
    {
        nameText.text = card.id;
        descriptionText.text = card.Description;
        manaText.text = card.Mana.ToString();
        attackText.text = card.Attack.ToString();
        healthText.text = card.Health.ToString();
        isPlayerOwn = card.isPlayerOwn;
    }
        
}
