using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TouchMoving : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject playerObject;
    public GameObject combineWindow;

    private GameObject placeHolder = null;
    private GameObject inventory, cardpile, cardDeck;
    private GameObject rivalGO;
    private List<GameObject> rivalNeighbours;

    private Card card;

    private Transform originalParent;

    private Vector2 grabOffset;

    Character player, rival;
    
    private bool isCombineCard;

    // --------------------------------------------------------------

    // --------------- İŞLEVLER --------------- 

    public void Start()
    {
        //Değişken atamaları
        inventory = GameObject.Find("Inventory").gameObject;
        cardpile = GameObject.Find("CardPile").gameObject;
        cardDeck = GameObject.Find("Deck").gameObject;

        player = PlayerData.player;
        // ----------------------------------------
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log(eventData.selectedObject);

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

        //player = playerObject.GetComponent<CharacterDisplay>().character;

        // --------------------------------------------------------------

        //Destedeki diğer kartları erişilemez hale getiriyor.
        for (int i = 0; i < cardDeck.transform.childCount; i++)
        {
            try
            {
                cardDeck.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            }
            catch
            {
                continue;
            }
        }

        //Kartın uygulanabileceği kimseleri belirtiyor.
        card.attackable_enemies(show:true);

        //Kartı aldıktan sonra collider bileşenini kapatıyor ki atılan yere giden ışını engellemesin
        this.GetComponent<BoxCollider2D>().enabled = false;

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

        //if(eventData.pointerEnter != null && eventData.pointerEnter.tag == "Enemy")
        //{
        //    rivalGO = eventData.pointerEnter;
        //    rivalNeighbours = findNeighbours(rivalGO, card.attackRegime);

        //    rivalNeighbours.Add(rivalGO);

        //    //rivalGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rivalGO.GetComponent<CharacterDisplay>().character.health + "-" + this.card.attack_range[0].ToString();
        //    int i = 0;
        //    foreach (GameObject rGO in rivalNeighbours)
        //    {
        //        rGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rGO.GetComponent<CharacterDisplay>().character.health + "-" + this.card.attack_range[i].ToString();
        //        i++;
        //    }

        //}
        
        //else
        //{
        //    try
        //    {
        //        rivalGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rivalGO.GetComponent<CharacterDisplay>().character.health.ToString();
        //        int i = 0;
        //        foreach (GameObject rGO in rivalNeighbours)
        //        {
        //            rGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rGO.GetComponent<CharacterDisplay>().character.health.ToString();
        //            i++;
        //        }
        //    }
        //    catch { }
        //    //rivalGO = eventData.pointerEnter;
        //    //rivalNeighbours = findNeighbours(rivalGO, card.attackRegime);

        //}


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter);

        rivalGO = eventData.pointerEnter;

        //Kartı attıktan sonra collider bileşenini açıyor ki bir daha alınabilsin
        this.GetComponent<BoxCollider2D>().enabled = true;

        //Kısayol
        GameObject hitObject =  eventData.pointerCurrentRaycast.gameObject;

        //Düşmanların hepsi görünür olsun.
        card.attackable_enemies(show: false);

        //Destedeki diğer kartları erişilebilir hale getiriyor.
        for (int i = 0; i < cardDeck.transform.childCount; i++)
        {
            try
            {
                cardDeck.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
            }
            catch
            {
                continue;
            }
        }

        //İleride kartın özelliğini etkilemek için kullanılıyor.
        bool is_card_used = false;

        //Eğer bir şeye çarpmadıysa geri dönsün
        if (hitObject == null)
        {
            returnToDeck();
            return;
        }


        //------------------------------------------------------------------------

        //Eğer düşman nesnesine atıldıysa
        if (hitObject.tag == "Enemy")
        {
            bool
                is_attack_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.AttackCard,
                is_buff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.BuffCard,
                is_debuff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DebuffCard;
            
            //Kısayol
            rival  = hitObject.GetComponent<CharacterDisplay>().character;
            //Debug.Log(rival);

            rivalNeighbours = findNeighbours(rivalGO, card.attackRegime);
            rivalNeighbours.Add(rivalGO);

            //Düşmanların toplandığı bir liste oluşturuluyor.
            List<Character> rivals = new List<Character>();

            //Kartı attığımız düşman içine ekleniyor.
            //rivals.Add(rival);
            //Varsa komuşları da ekleniyor.

            foreach (GameObject rivGO in rivalNeighbours)
            {
                Character rivalChar = rivGO.GetComponent<CharacterDisplay>().character;
                rivals.Add(rivalChar);
            }

            switch (card.CardT1)
            {
                case CardType1.AttackCard:
                    // Kartın yapacağı saldırı fonksiyonu çağırılıyor.
                    this.GetComponent<AttackCard>().attack(player, rivals.ToArray(), rivalNeighbours.ToArray());

                    // Kartın etkilediği düşman(lar)ın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().situationUpdater();
                    foreach (GameObject rivalObjs in rivalNeighbours)
                    {
                        rivalObjs.GetComponent<CharacterDisplay>().situationUpdater();
                    }

                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().situationUpdater();
                    // Kullanılan kartın silinmesi için
                    is_card_used = true;
                    break;

                case CardType1.DebuffCard:
                    // Kartın yapacağı düşürücü fonksiyonu çağırılıyor.
                    this.GetComponent<DebuffCard>().debuffApplier(rival);
                    // Kartın etkilediği düşmanın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().situationUpdater();
                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().situationUpdater();
                    // Kullanılan kartın silinmesi için
                    is_card_used = true;
                    break;

                case CardType1.SpecialCard:

                    is_card_used = true;
                    break;

                case CardType1.UnusualCard:

                    is_card_used = true;
                    break;
            }

            switch (card.CardT2)
            {
                case CardType2.BuffCard:
                    // Kartın yapacağı arttırıcı fonksiyonu çağırılıyor.
                    this.GetComponent<BuffCard>().buffApplier(player);
                    // Kartın etkilediği düşmanın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().situationUpdater();
                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().situationUpdater();
                    // Kullanılan kartın silinmesi için
                    is_card_used = true;
                    break;

                case CardType2.DebuffCard:
                    // Kartın yapacağı düşürücü fonksiyonu çağırılıyor.
                    this.GetComponent<DebuffCard>().debuffApplier(rival);
                    // Kartın etkilediği düşmanın can kontrolü
                    hitObject.GetComponent<CharacterDisplay>().situationUpdater();
                    // Kartın etkilediği oyuncunun değerlerin ekrana yazdırılması
                    playerObject.GetComponent<CharacterDisplay>().situationUpdater();
                    // Kullanılan kartın silinmesi için
                    is_card_used = true;
                    break;

                case CardType2.None:
                    break;
            }

            //Eğer kart kullanılmışsa
            if (is_card_used)
            {
                //Kartın tutacağını yok etme
                Destroy(placeHolder);
                
                //Kartın kullanıldığını belirtmek için değişkeni doğru olarak atanıyor.
                this.gameObject.GetComponent<CardDisplay>().isCardUsed = true;

                //kullanılma sayısını arttırıyor.
                card.timesUsed++;

                //Aşağıdaki durumlarda kart bir daha kullanılamaz hale getiriliyor.
                if (card.cardUsage == CardUsage.Delicate && card.timesUsed > 0)
                    card.canCardUsable = false;
                else if (card.cardUsage == CardUsage.DelicatePlus && card.timesUsed > 1)
                    card.canCardUsable = false;
                else if (card.cardUsage == CardUsage.Consumable && card.timesUsed > 0)
                    card.canCardUsable = false;
                    
                //Limandan siliyor.
                cardDeck.GetComponent<DeckScript>().cardsInDeck.Remove(card);

                //Kullanılmış kart destesine ekliyor.
                cardpile.GetComponent<CardPileScript>().CardPile.Add(card);
                
                //Oyun nesnesini siliyor.
                Destroy(gameObject);
            }

            else
                returnToDeck();

            
            //Manası yeterli olmayan kartları kapatacak fonksiyon çağırılıyor.
            playerObject.GetComponent<CharacterDisplay>().cardRequirements(player);
        }

        //Eğer oyuncu nesnesine çarptıysa 
        else if (hitObject.tag == "Player")
        {
            bool
                is_defence_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DefenceCard,
                is_energy_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.EnergyCard,
                is_buff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.BuffCard,
                is_debuff_card = GetComponent<CardDisplay>().card.CardT1 == CardType1.DebuffCard;


            switch (card.CardT1)
            {
                case CardType1.DefenceCard:
                    player.shieldApply(card.defence);
                    player.consumeMana(card.mana);
                    playerObject.GetComponent<CharacterDisplay>().situationUpdater();
                    is_card_used = true;
                    break;

                case CardType1.BuffCard:
                    this.GetComponent<BuffCard>().buffApplier(player);
                    player.consumeMana(card.mana);
                    playerObject.GetComponent<CharacterDisplay>().situationUpdater();
                    is_card_used = true;
                    break;

                case CardType1.EnergyCard:
                    player.energyApply(card.mana);
                    playerObject.GetComponent<CharacterDisplay>().situationUpdater();
                    is_card_used = true;
                    break;

                case CardType1.SpecialCard:

                    is_card_used = true;
                    break;

                case CardType1.UnusualCard:
                    
                    is_card_used = true;
                    break;
            }

            switch (card.CardT2)
            {
                case CardType2.BuffCard:
                    this.GetComponent<BuffCard>().buffApplier(player);
                    is_card_used = true;
                    break;

                case CardType2.DebuffCard:
                    this.GetComponent<DebuffCard>().debuffApplier(player);
                    is_card_used = true;
                    break;

                case CardType2.None:
                    break;
            }

            if (is_card_used)
            {
                //Kartı yok etme
                Destroy(placeHolder);

                //kullanılma sayısını arttırıyor.
                card.timesUsed++;

                //Aşağıdaki durumlarda kart bir daha kullanılamaz hale getiriliyor.
                if (card.cardUsage == CardUsage.Delicate && card.timesUsed > 0)
                    card.canCardUsable = false;
                else if (card.cardUsage == CardUsage.DelicatePlus && card.timesUsed > 1)
                    card.canCardUsable = false;
                else if (card.cardUsage == CardUsage.Consumable && card.timesUsed > 0)
                    card.canCardUsable = false;

                Destroy(gameObject);
            }
            else
                returnToDeck();


            //Manası yeterli olmayan kartları kapatacak fonksiyon çağırılıyor.
            playerObject.GetComponent<CharacterDisplay>().cardRequirements(player);
        }
        
        //Eğer birleştiriciye çarptıysa
        else if(hitObject.tag == "CardCombiner")
        {
            //Kartı atama işlemi
            transform.SetParent(hitObject.transform);
            
            //Eski yerdeki boş yeri siliyor.
            Destroy(placeHolder);

            //Kart yerlerini kontrol ediyor.
            hitObject.GetComponentInParent<CombineWindowMainScript>().checks_card_slots();

            //Eğer iki kart yeri de doluysa, önizleme olarak kartı sonuç kısmına koysun.
            if (!hitObject.GetComponentInParent<CombineWindowMainScript>().isLeftCardSlotEmpty && !hitObject.GetComponentInParent<CombineWindowMainScript>().isLeftCardSlotEmpty)
            {
                hitObject.GetComponentInParent<CombineWindowMainScript>().Combine_them();
            }
            //return;
        }

        //Eğer Kart destesine çarptıysa
        else if (hitObject.tag == "CardDeck")
        {
            //Desteye erişim
            Transform deck = hitObject.transform;
            //Deste içine atıyor.
            transform.SetParent(deck);
            //Destedeki sırasını belirliyor.
            transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());

            //Eski yerdeki boş yeri siliyor.
            Destroy(placeHolder);
        }

        else
        {
            returnToDeck();
        }

        //------------------------------------------------------------------------

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

    private int[] findNeighboursIndex(int i, AttackRegime ar)
    {
        switch (ar)
        {
            case AttackRegime.Horizontal:
                switch (i)
                {
                    case 0:
                        return (new int[] { 1 });
                    case 1:
                        return (new int[] { 0 });
                    case 2:
                        return (new int[] { 3 });
                    case 3:
                        return (new int[] { 4 });
                    default:
                        return new int[] { };
                }

            case AttackRegime.Vertical:
                switch (i)
                {
                    case 0:
                        return (new int[] { 2 });
                    case 1:
                        return (new int[] { 3 });
                    case 2:
                        return (new int[] { 0 });
                    case 3:
                        return (new int[] { 1 });
                    default:
                        return new int[] { };
                }

            case AttackRegime.Triangle:
                switch (i)
                {
                    case 0:
                        return (new int[] { 1, 2 });
                    case 1:
                        return (new int[] { 0, 3 });
                    case 2:
                        return (new int[] { 0, 3 });
                    case 3:
                        return (new int[] { 1, 2 });
                    default:
                        return new int[] { };
                }

            case AttackRegime.AllRegions:
                switch (i)
                {
                    case 0:
                        return (new int[] { 1, 2, 3 });
                    case 1:
                        return (new int[] { 0, 2, 3 });
                    case 2:
                        return (new int[] { 0, 1, 3 });
                    case 3:
                        return (new int[] { 0, 1, 2 });
                    default:
                        return new int[] { };
                }

            default:
                return new int[] { };
        }
        
    }

    private List<GameObject> findNeighbours(GameObject rival, AttackRegime ar)
    {
        List<GameObject> rivalNgs = new List<GameObject>();

        int rivalIndex = rival.transform.GetSiblingIndex();
        int[] neighbourIndexes = findNeighboursIndex(rivalIndex, ar);

        foreach (int index in neighbourIndexes)
        {
            try
            { 
                if (rival.transform.parent.GetChild(index) != null)
                    rivalNgs.Add(rival.transform.parent.GetChild(index).gameObject);
            }
            catch
            {
                
            }
        }

        return rivalNgs;
    }

}
