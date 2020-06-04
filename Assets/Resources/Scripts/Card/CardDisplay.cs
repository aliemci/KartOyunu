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

    public Material toggleOn, toggleOff;

    public bool isPlayerOwn, isCardUsed;

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
        //Uygun çözünürlüğe getirmek için katsayılar ile çarpılıyor.
        transform.localScale = transform.localScale * resolution_scale * resolution_scale;
        //z ölçeğini 0 yapıyor. Bunu engellemek için tekrardan 0,0,1 vektörü ile topladım.
        transform.localScale = transform.localScale + new Vector3(0f, 0f, 1f);
    }

    public void Refresh()
    {
        nameText.text = card.cardName;
        attackText.text = card.attack.ToString();
        shieldText.text = card.defence.ToString();
        manaText.text = card.mana.ToString();
        isPlayerOwn = card.isPlayerOwn;
        isCardUsed = card.isCardUsed;
    }

    public void toggle_card(bool boolForToggleCard)
    {
        if (boolForToggleCard)
        {
            //Kartın içindeki bütün render parçasına sahip elemanları yarı saydam yapacak.
            foreach(Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                Color newColor = renderer.material.color;
                newColor.a = 1f;
                renderer.material.color = newColor;
            }

            //Kartın içindeki yazıları yarı saydam yapacak.
            foreach (TextMeshProUGUI tmp in this.GetComponentsInChildren<TextMeshProUGUI>())
            {
                Color newColor = tmp.color;
                newColor.a = 1f;
                tmp.color = newColor;
            }

            //Hareket etmesini engelleyecek.
            this.GetComponent<TouchMoving>().enabled = true;
        }
        else
        {
            //Kartın içindeki bütün render parçasına sahip elemanları opak yapacak.
            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                Color newColor = renderer.material.color;
                newColor.a = 0.3f;
                renderer.material.color = newColor;
            }

            //Kartın içindeki yazıları opak yapacak.
            foreach (TextMeshProUGUI tmp in this.GetComponentsInChildren<TextMeshProUGUI>())
            {
                Color newColor = tmp.color;
                newColor.a = 0.3f;
                tmp.color = newColor;
            }

            //Hareket etmesini engelleyecek.
            this.GetComponent<TouchMoving>().enabled = false;
        }
    }
}
