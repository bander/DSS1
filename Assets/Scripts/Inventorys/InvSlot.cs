using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InvSlot : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler,IDropHandler,IPointerEnterHandler,IPointerExitHandler {
    InvManager manager;
    CanvasController canvas;

    Item item;
    public Image icon;
    public GameObject highLightIcon;
    public GameObject countText;
    public int invIndex;
    public int slotIndex;

    int countItems;
    int maxcountOfItems;

    Transform parentToReturn;
    InvSlot placeHolder;

    public EquipmentSlot slotType = EquipmentSlot.None;
    
    bool isDragging;
    void Start()
    {
        manager = InvManager.instance;
        canvas = CanvasController.instance;
    }

    public void SetItem(Item newItem,int newIndex,int newInvIndex)
    {
        if (newItem != null)
        {
            item = newItem.Clone();
            icon.sprite = item.icon;
            icon.enabled = true;
            transform.SetSiblingIndex(slotIndex);
            if (item.countInSlot != 1)
            {
                countText.SetActive(true);
                countText.GetComponent<TMP_Text>().text = item.countInSlot.ToString();
            }
            else
            {
                countText.SetActive(false);
            }
        }
        else
        {
            Clear();
        }

        slotIndex = newIndex;
        invIndex = newInvIndex;
    }
    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;

        if (highLightIcon.activeSelf) highLight(false);
        countText.SetActive(false);
    }

    public void highLight(bool activate=true)
    {
        highLightIcon.SetActive(activate);
    }
    public Item GetItem()
    {
        return item;
    }

    public void SelectSlot()
    {
        if (canvas.selectedSlot != null)
        {
            if (canvas.selectedSlot == this)
            {
                canvas.selectedSlot.DeselectSlot();
                return;
            }
            canvas.selectedSlot.DeselectSlot();
        }
        
        if (item != null)
        {
            canvas.selectedSlot = this;

            bool splitted = (item.countInSlot > 1) ? true : false;
            if (splitted)
            {
                if(manager.invents[0].findIndexEmptySlot()==-1) splitted=false;
            }

            highLight();
            CanvasController.instance.buttonsControl(item.isUsable, splitted, true);
        }
        //*/
    }
    public void DeselectSlot()
    {
        highLight(false);
        canvas.selectedSlot = null;
        canvas.buttonsControl(false, false, false);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            if (highLightIcon.activeSelf) DeselectSlot();

            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

            placeHolder = Instantiate(this,this.transform).GetComponent<InvSlot>();
            placeHolder.transform.parent = this.transform.parent;
            placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            placeHolder.countText.SetActive(false);
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
        if (highLightIcon.activeSelf)
        {
            highLight(false);
        }
        if (eventData.pointerDrag != null && isDragging)
        {
            if (placeHolder != null)
            {
                Destroy(placeHolder.gameObject);
            }
            this.transform.SetParent(parentToReturn);
            transform.SetSiblingIndex(slotIndex);

            if (item==null) 
            {
                countText.SetActive(false);
            }else if (item.countInSlot == 1)
            {
                countText.SetActive(false);
            }

        }
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    

    public void OnDrop(PointerEventData eventData)
    {
        bool check = checkDragData(eventData);
        if (check)
        {
            InvSlot slt = eventData.pointerDrag.GetComponent<InvSlot>();
            if (highLightIcon.activeSelf)
            {
                if (item==null)
                {
                    InvManager.instance.invents[slt.invIndex].SwitchItems(slt.slotIndex, slotIndex,slt.invIndex,invIndex);
                }
                else
                {
                    InvManager.instance.invents[slt.invIndex].fillItems(slt.slotIndex, slotIndex, slt.invIndex, invIndex);
                }
                highLight(false);
            }
        }
        InvManager.instance.OnInvChangedCallback.Invoke();
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        bool check = checkDragData(eventData);
        if (check)
        {
            if(item == null)
            {
                highLight();
            }
            else{
                int availMergeCount = checkEqualsItemAndAvailableCount(eventData);
                if (availMergeCount > 0)
                {
                    highLight();
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bool check = checkDragData(eventData);
        if (check)
        {
            if (item == null)
            {
                highLight(false);
            }
            else
            {
                int availMergeCount = checkEqualsItemAndAvailableCount(eventData);
                if (availMergeCount > 0)
                {
                    highLight(false);
                }

            }

        }

    }

    int checkEqualsItemAndAvailableCount(PointerEventData eventData)
    {
        int ret = 0;
        if (slotType == EquipmentSlot.None)
        {
            Item dragItem = eventData.pointerDrag.GetComponent<InvSlot>().item;
            if (dragItem.type == item.type)
            {
                if (item.countInSlot < item.maxCountInSlot)
                {
                    ret = item.maxCountInSlot - item.countInSlot;
                }
            }
        }

        return ret;
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
                    if (slotType == EquipmentSlot.None)
                    {
                        return true;
                    }else
                    {
                        if ((dragSlot.item as Equipment)!=null){
                            if (slotType == (dragSlot.item as Equipment).equipSlot || slotType == (dragSlot.item as Equipment).equipSlot2){
                                return true;
                            }
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
