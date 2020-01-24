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

    void Start()
    {
        nameText.text = card.Name;
        descriptionText.text = card.Description;
        manaText.text = card.Mana.ToString();
        attackText.text = card.Attack.ToString();
        healthText.text = card.Health.ToString();
    }
    
}
