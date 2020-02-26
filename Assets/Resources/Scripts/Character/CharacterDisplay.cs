using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterDisplay : MonoBehaviour
{
    public Character playableChar;
    string Id;
    int Health, Mana, Shield, maxHealth;
    Sprite CharacterSprite;

    //Her ekran için ayrı büyüklükte olacağı için
    private Vector2 resolution_scale;

    void Awake()
    {
        resolution_scale = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale;
    }

    void Start()
    {
        //Düşman bilgileri ekleniyor.
        Refresh();
        //Ekrana göre büyüklükleri ayarlanıyor.
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
        //Can yazısı yazdırılıyor.
        GameObject.Find("Heart_Value").GetComponent<TextMeshProUGUI>().text = Health.ToString() + "/" + maxHealth.ToString();
    }

    public void Refresh()
    {
        //Düşman bilgileri ataması
        Id = playableChar.Id;
        Health = playableChar.Health;
        maxHealth = Health;
        Mana = playableChar.Mana;
        Shield = playableChar.Shield;
        GetComponent<SpriteRenderer>().sprite = playableChar.CharacterSprite;
    }
}
