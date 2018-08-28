using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InvSlot : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler,IDropHandler,IPointerEnterHandler,IPointerExitHandler {
    InvManager manager;

    Item item;
    public Image icon;
    public Image highLightIcon;
    public GameObject countText;
    int invIndex;
    int slotIndex;

    int countItems;
    int maxcountOfItems;

    Transform parentToReturn;
    InvSlot placeHolder;

    public EquipmentSlot slotType = EquipmentSlot.None;

    bool isDragging;
    void Start()
    {
        manager = InvManager.instance;
        //  countText.GetComponent<Text>().text = "12";
    }

    public void SetItem(Item newItem,int index,int newInvIndex)
    {
        
        if (newItem != null)
        {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
            transform.SetSiblingIndex(slotIndex);

        }
        else
        {
            Clear();
        }

        slotIndex = index;
        invIndex = newInvIndex;
    }
    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void highLight(bool activate=true)
    {
        highLightIcon.enabled = activate;
    }




    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

            placeHolder = Instantiate(this,this.transform).GetComponent<InvSlot>();
            placeHolder.transform.parent = this.transform.parent;
            placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            placeHolder.Clear();

            parentToReturn = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent.parent.parent);

            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null && isDragging)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (highLightIcon.enabled)
        {
            highLight(false);
        }
        if (eventData.pointerDrag != null && isDragging)
        {
            Destroy(placeHolder.gameObject);
            this.transform.SetParent(parentToReturn);
            transform.SetSiblingIndex(slotIndex);
            isDragging = false;
        }
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        bool check = checkDragData(eventData);
        if (check)
        {
            InvSlot slt = eventData.pointerDrag.GetComponent<InvSlot>();
            if (highLightIcon.enabled)
            {
                InvManager.instance.invents[slt.invIndex].SwitchItems(slt.slotIndex, slotIndex,slt.invIndex,invIndex);
                highLight(false);
            }
        }
        InvManager.instance.OnInvChangedCallback.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bool check = checkDragData(eventData);
        if (item == null && check)
        {
            highLight();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bool check = checkDragData(eventData);
        if (item == null && check)
        {
            highLight(false);
        }

    }

    bool checkDragData(PointerEventData eventData)
    {
        GameObject dragData = eventData.pointerDrag;
        if (dragData != null)
        {
            InvSlot dragSlot = dragData.GetComponent<InvSlot>();
            if (dragSlot != null)
            {
                if (dragSlot.item != null)
                {
                    if (slotType==EquipmentSlot.None)
                    {
                        return true;
                    }else
                    {
                        if ((dragSlot.item as Equipment)!=null && slotType == (dragSlot.item as Equipment).equipSlot)
                        {
                            return true;
                        }
                        return false;
                    }

                }
                return false;
            }
            return false;
        }
        return false;
    }
}

public enum SlotTypes {Default,Equip,CraftIn,CraftOut }
