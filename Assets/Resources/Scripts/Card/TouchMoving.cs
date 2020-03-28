﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TouchMoving : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    Transform originalParent;
    Vector2 grabOffset;

    GameObject placeHolder = null;

    playerCharacter player;
    rivalCharacter rival;

    public void Start()
    {
        player = GameObject.Find("ScriptHolder").GetComponent<CharacterGenerator>().characters[0] as playerCharacter;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Kartın destedeki yerini tutacak bir boşluk oluşturuluyor.
        placeHolder = new GameObject();

        //Destenin çocuğu olarak ayarlanıyor.
        placeHolder.transform.SetParent(this.transform.parent);

        //Yatay düzende durabilmesi için "LayoutElement" eklentisi ekleniyor.
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();

        //Asıl kartın yüksekliği genişliğini alıyor.
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;

        //Kartın olduğu yere yerleşiyor.
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        //Kartı tuttuğu yer bir değişkene atılıyor.
        grabOffset =  transform.position - Input.mousePosition;
        
        //Desteye geri döndürebilmek için
        originalParent = this.transform.parent;
        
        //Kartı desteden çıkarıyor.
        this.transform.SetParent(this.transform.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //transform.position = Vector3.Lerp(transform.position, Input.GetTouch(0).position + grabOffset, Time.deltaTime * 5);
            transform.position = Input.GetTouch(0).position + grabOffset;
        }
        else
        {
            Vector3 grabOffset3 = grabOffset;
            //transform.position = Vector3.Lerp(transform.position, Input.mousePosition + grabOffset3, Time.deltaTime * 5);
            transform.position = Input.mousePosition + grabOffset3;
        }

        //Kartın gideceği yeri belirlemek adına kullanılan bir değişken.
        int newSiblingIndex = originalParent.childCount;

        //Destedeki her kart için döngüye giriyor.
        for(int i=0; i<originalParent.childCount; i++)
        {
            // x konumlarına bakıyor. Yatay doğru.
            if(this.transform.position.x < originalParent.GetChild(i).position.x)
            {
                //konumu bir değişkene atıyor.
                newSiblingIndex = i;

                //Konumunu buluyor.
                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }
        //Son olarak konumu ayarlıyor.
        placeHolder.transform.SetSiblingIndex(newSiblingIndex);

    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        
        //Fare'nin konumundan bir ışın gönderiliyor.
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up, 500f);

        GameObject hitObject = hit.collider.gameObject;

        Debug.Log(hitObject.layer);

        //Eğer düşman objesine çarptıysa
        if(hitObject.layer == 8)
        {
            bool
                is_attack_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.AttackCard,
                is_buff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.BuffCard,
                is_debuff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DebuffCard;
            

            rival  = hitObject.GetComponent<CharacterDisplay>().character as rivalCharacter;
                       
            if (is_attack_card)
            {
                //Mana kontrolü
                if(player.mana >= GetComponent<AttackCard>().Mana)
                {
                    //Düşmanın üstüne isabet ettiyse "DamageTaken" işlevi çağırılıyor.
                    hitObject.GetComponent<CharacterDisplay>().DamageTaken(GetComponent<AttackCard>().Attack);

                    if (is_buff_card)
                    {   //Bu fonksiyon character classından nesne alıyor. O yüzden character display kodundan ona erişiyorum.
                        GetComponent<BuffCard>().buffApplier(player);
                    }

                    //Mana tüketimi
                    player.mana -= GetComponent<AttackCard>().Mana;

                    //Kartı yok etme
                    Destroy(placeHolder);
                    Destroy(gameObject);
                }
            }
           

        }

        else if(hitObject.layer == 9)
        {
            bool
                is_defence_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DefenceCard,
                is_energy_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.EnergyCard,
                is_buff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.BuffCard,
                is_debuff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DebuffCard;

            if (is_defence_card)
            {
                //Mana kontrolü
                if (player.mana >= GetComponent<AttackCard>().Mana)
                {
                    //Düşmanın üstüne isabet ettiyse "DamageTaken" işlevi çağırılıyor.
                    hitObject.GetComponent<CharacterDisplay>().DamageTaken(GetComponent<AttackCard>().Attack);

                    if (is_buff_card)
                    {   //Bu fonksiyon character classından nesne alıyor. O yüzden character display kodundan ona erişiyorum.
                        GetComponent<BuffCard>().buffApplier(player);
                    }

                    //Mana tüketimi
                    player.mana -= GetComponent<AttackCard>().Mana;

                    //Kartı yok etme
                    Destroy(placeHolder);
                    Destroy(gameObject);
                }
            }
        }
        
        else
        {
            //Kartı desteye koyma
            this.transform.SetParent(originalParent);
            this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        }


        Destroy(placeHolder);
        
    }
}