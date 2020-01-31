using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchMoving : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    Vector2 grabOffset;

    GameObject placeHolder = null;
    

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

        int newSiblingIndex = originalParent.childCount;

        for(int i=0; i<originalParent.childCount; i++)
        {
            if(this.transform.position.x < originalParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if(placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }

        placeHolder.transform.SetSiblingIndex(newSiblingIndex);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Fare'nin konumuna bir ışın gönderiliyor.
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up, 500f);

        //Işın bir düşmana çarptıysa:(Layer 8 == Düşman)
        if (hit.collider.gameObject.layer == 8)
        {
            //Düşmanın üstüne isabet ettiyse "DamageTaken" işlevi çağırılıyor.
            hit.collider.gameObject.GetComponent<EnemyDisplay>().DamageTaken(GetComponent<CardDisplay>().card.Attack);
        }

        //Kartı desteye koyma
        this.transform.SetParent(originalParent);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());

        Destroy(placeHolder);
    }
}
