using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TouchMoving : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject playerObject, rivalObject;
    
    Card card;

    Transform originalParent;

    Vector2 grabOffset;

    GameObject placeHolder = null;

    Character player, rival;
    
    private bool isCombineCard;

    public GameObject combineWindow;

    // --------------------------------------------------------------

    // --------------- İŞLEVLER --------------- 

    public void Start()
    {
        
        // ----------------------------------------
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Eğer UI katmanındaysa (Kartların olduğu liman UI o yüzden)
        if(this.transform.parent.gameObject.layer == 5 || true)
        {
            // KARTIN DESTEDEN ALINMASI İŞLEMİ ------------------------------
        
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

            placeHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(le.preferredWidth, le.preferredHeight);

            //Kartın olduğu yere yerleşiyor.
            placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            //Kartı tuttuğu yer bir değişkene atılıyor.
            grabOffset =  transform.position - Input.mousePosition;
        
            //Desteye geri döndürebilmek için
            originalParent = this.transform.parent;
        
            //Kartı desteden çıkarıyor.
            this.transform.SetParent(this.transform.parent.parent);

        }

        // --------------------------------------------------------------

        //Değişken atamaları
        card = this.GetComponent<CardDisplay>().card;

        playerObject = GameObject.Find("Player");

        player = playerObject.GetComponent<CharacterDisplay>().character;

        // --------------------------------------------------------------
        

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Mobil
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            transform.position = Input.GetTouch(0).position + grabOffset;
        }
        //PC
        else
        {
            Vector3 grabOffset3 = grabOffset;
            transform.position = Input.mousePosition + grabOffset3;
        }

        // --------------------------------------------------------------


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

        // --------------------------------------------------------------



    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Fare'nin konumundan bir ışın gönderiliyor.
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up, 500f);

        //Kısayol
        GameObject hitObject;
        

        try
        {
            Debug.Log(hit.collider.gameObject.name);
            //Eğer dönen obje yoksa hata verecektir.
            hitObject = hit.collider.gameObject;
        }
        catch
        {
            //Hata gelirse kartı deck'e geri koysun.
            returnToDeck();
            return;
        }

        //İleride kartın yok edilip edilmemesini kontrol için kullanılıyor.
        bool should_card_destory = false;

        //------------------------------------------------------------------------

        //Eğer düşman nesnesine atıldıysa
        if (hitObject.layer == 8)
        {
            bool
                is_attack_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.AttackCard,
                is_buff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.BuffCard,
                is_debuff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DebuffCard;
            

            rival  = hitObject.GetComponent<CharacterDisplay>().character;
            Debug.Log(rival);

            switch (card.CardT1)
            {
                case CardType1.AttackCard:
                    // Kartın yapacağı saldırı fonksiyonu çağırılıyor.
                    this.GetComponent<AttackCard>().attack(player, rival);
                    // Kartın etkilediği düşmanın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().checkIsDead();
                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().healthManaWriter();
                    // Kullanılan kartın silinmesi için
                    should_card_destory = true;
                    break;

                case CardType1.DebuffCard:
                    // Kartın yapacağı düşürücü fonksiyonu çağırılıyor.
                    this.GetComponent<DebuffCard>().debuffApplier(rival);
                    // Kartın etkilediği düşmanın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().checkIsDead();
                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().healthManaWriter();
                    // Kullanılan kartın silinmesi için
                    should_card_destory = true;
                    break;

                case CardType1.SpecialCard:

                    should_card_destory = true;
                    break;

                case CardType1.UnusualCard:

                    should_card_destory = true;
                    break;
            }

            switch (card.CardT2)
            {
                case CardType2.BuffCard:
                    // Kartın yapacağı arttırıcı fonksiyonu çağırılıyor.
                    this.GetComponent<BuffCard>().buffApplier(player);
                    // Kartın etkilediği düşmanın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().checkIsDead();
                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().healthManaWriter();
                    // Kullanılan kartın silinmesi için
                    should_card_destory = true;
                    break;

                case CardType2.DebuffCard:
                    // Kartın yapacağı düşürücü fonksiyonu çağırılıyor.
                    this.GetComponent<DebuffCard>().debuffApplier(rival);
                    // Kartın etkilediği düşmanın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().checkIsDead();
                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().healthManaWriter();
                    // Kullanılan kartın silinmesi için
                    should_card_destory = true;
                    break;

                case CardType2.None:
                    break;
            }

            if (should_card_destory)
            {
                //Kartı yok etme
                Destroy(placeHolder);
                Destroy(gameObject);
            }
            else
                returnToDeck();

            /*
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
            */

            //Manası yeterli olmayan kartları kapatacak fonksiyon çağırılıyor.
            playerObject.GetComponent<CharacterDisplay>().cardRequirements(player);
        }

        //Eğer oyuncu nesnesine çarptıysa
        else if (hitObject.layer == 9)
        {
            bool
                is_defence_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DefenceCard,
                is_energy_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.EnergyCard,
                is_buff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.BuffCard,
                is_debuff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DebuffCard;


            switch (card.CardT1)
            {
                case CardType1.DefenceCard:
                    should_card_destory = true;
                    break;

                case CardType1.BuffCard:

                    should_card_destory = true;
                    break;

                case CardType1.SpecialCard:

                    should_card_destory = true;
                    break;

                case CardType1.UnusualCard:

                    should_card_destory = true;
                    break;
            }

            switch (card.CardT2)
            {
                case CardType2.BuffCard:
                    this.GetComponent<BuffCard>().buffApplier(player);
                    should_card_destory = true;
                    break;

                case CardType2.DebuffCard:

                    should_card_destory = true;
                    break;

                case CardType2.None:
                    break;
            }

            if (should_card_destory)
            {
                //Kartı yok etme
                Destroy(placeHolder);
                Destroy(gameObject);
            }
            else
                returnToDeck();

            /*
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
            */

            //Manası yeterli olmayan kartları kapatacak fonksiyon çağırılıyor.
            playerObject.GetComponent<CharacterDisplay>().cardRequirements(player);
        }
        
        //Eğer birleştiriciye çarptıysa
        else if(hitObject.layer == 10)
        {
            //Kartı atama işlemi
            transform.SetParent(hit.collider.transform);
            
            //Eski yerdeki boş yeri siliyor.
            Destroy(placeHolder);
            //return;
        }

        //Eğer Kart destesine çarptıysa
        else if (hitObject.layer == 11)
        {
            //Desteye erişim
            Transform deck = hitObject.transform;
            transform.SetParent(deck);

            //Eski yerdeki boş yeri siliyor.
            Destroy(placeHolder);
        }
        
        else
            returnToDeck();
        //------------------------------------------------------------------------


        //Manası yeterli olmayan kartları kapatacak fonksiyon çağırılıyor.
        //playerObject.GetComponent<CharacterDisplay>().cardRequirements(player);
    }

    public void returnToDeck()
    {
        //Kartı desteye koyma
        this.transform.SetParent(originalParent);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        Destroy(placeHolder);
    }

    public void createCombinerWindow()
    {
        //Öge oluşturuluyor. İsmi konuyor. Aktif hale getiriliyor.
        GameObject windowForCombine = Instantiate(combineWindow);
        windowForCombine.name = "CombineCardPanel";
        windowForCombine.SetActive(true);

        //Gözükmesi için kanvas içine alınıyor. 100 birim yukarıya konumlandırılıyor.
        windowForCombine.transform.SetParent(GameObject.Find("Canvas").transform);
        windowForCombine.transform.localPosition = Vector3.zero + new Vector3(0f, 100f, 0f);

        //------------------------------------------------------------------------

        //Yanlışlıkla düşmanlara basılmasın diye "collider" kapatılıyor.
        for (int i=0; i< GameObject.Find("Enemy-deck").transform.childCount; i++)
            GameObject.Find("Enemy-deck").transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;

        //Yanlışlıkla oyuncuya basılmasın diye "collider" kapatılıyor.
        for (int i = 0; i < GameObject.Find("Player-deck").transform.childCount; i++)
            GameObject.Find("Player-deck").transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;

        //------------------------------------------------------------------------

        //Kartlardaki birleşme tuşunu çalışır vaziyete getiriyor.
        foreach (GameObject cardObj in GameObject.FindGameObjectsWithTag("Card"))
        {
            if (cardObj.GetComponent<CombineCard>())
            {
                cardObj.transform.Find("Combine").gameObject.SetActive(false);
            }
        }


    }
    

}
