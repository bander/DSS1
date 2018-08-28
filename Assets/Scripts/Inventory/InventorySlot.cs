using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    Item item;

    public EquipmentSlot equipmentSlot;



    public bool isEmpty = true;
    public Transform returnParent;
    int index;

    public Transform canvas;
    InventorySlot placeHolder;
    InventorySlot placeOnPointerEnter;


    void Start()
    {
        canvas = FindObjectOfType<CanvasController>().gameObject.transform;
    }

    public void AddItem(Item newItem)
    {
        if (newItem != null)
        {

            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;
            isEmpty = false;
        }
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        isEmpty = true;
    }

    public void UseItem()
    {

    }

    void createHolder()
    {
        placeHolder = Instantiate(this.gameObject,this.transform).GetComponent<InventorySlot>();
        placeHolder.transform.SetParent(this.transform.parent);
        placeHolder.transform.SetSiblingIndex(index);

        placeHolder.GetComponent<InventorySlot>().ClearSlot();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isEmpty)
        {
            index = this.transform.GetSiblingIndex();
            returnParent = this.transform.parent;

            createHolder();

            this.transform.parent = canvas;// this.transform.parent.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isEmpty)
        {
            Destroy(gameObject);
        }
        Inventory.instance.OnItemChangedCallback();
        /*InventorySlot pp = eventData.pointerDrag.GetComponent<InventorySlot>();//

        if (pp != null)
        {
            placeHolder = pp;
            index = placeHolder.transform.GetSiblingIndex();
            Destroy(pp);
        }
      

        this.transform.parent = returnParent;
        this.transform.SetSiblingIndex(index);

        Destroy(placeHolder);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isEmpty)
        {
            transform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (slot != null && !slot.isEmpty) {
            if (equipmentSlot == EquipmentSlot.None)
            {
                Debug.Log("cch "+ slot.index+' '+this.transform.GetSiblingIndex());

                //Inventory.instance.replaceItem(slot.index, this.transform.GetSiblingIndex());
                /*if (isEmpty)
                {
                    AddItem(slot.item);
                }
                else
                {
                    slot.placeHolder.AddItem(slot.item);
                }
                //*/
            }
            else
            {

            }
           
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        
        if (isEmpty)
        {
          //  eventData.pointerDrag.GetComponent<InventorySlot>().placeOnPointerEnter = this;
        }
        else
        {
           // eventData.pointerDrag.GetComponent<InventorySlot>().placeOnPointerEnter = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        
        if (isEmpty)//  && !disabled)
        {
            //eventData.pointerDrag.GetComponent<InventorySlot>().placeOnPointerEnter = null;
        }

    }
    
}