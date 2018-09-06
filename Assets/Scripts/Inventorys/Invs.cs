    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invs : MonoBehaviour {
    InvManager manager;
    CanvasController canvas;

    [SerializeField]
    List<InvSlot> slots;
    int num = 0;

    public GameObject slotPrefab;

    List<int> selectedSlots;

    void Start()
    {
        manager = InvManager.instance;
        canvas = CanvasController.instance;

        if (slots == null)
        {
           // slots = GetComponentsInChildren<InvSlot>();
        }
        activateBackpackSlots();
    }
    public void SetNum(int number)
    {
        if (manager == null)
        {
            manager = InvManager.instance;
        }
        num = number;
        manager.OnInvChangedCallback += UpdateSlots;

        int i = 0;
        foreach (InvSlot s in slots)
        {
            s.SetItem(null, i, num);
            i++;
        }
    }
    void UpdateSlots()
    {
        int i = 0;
        Invent invent = manager.GetInvent(num);
        if (invent != null)
        {
            foreach (InvSlot s in slots)
            {
                if (invent.items.Count > i)
                {
                    Item tm = invent.items[i];
                    if (tm != null)
                    {
                        s.SetItem(tm.Clone(), i, num);
                    }
                    else
                    {
                        s.SetItem(null, i, num);
                    }
                }

                i++;
            }
        }
    }

    public void highlightAllEmptyAndType()
    {
        /*int sel = canvas.selectedSlot.slotIndex;
        Invent invent = manager.invents[num];

        selectedSlots = invent.findAllIndexesEmptySlots();
        List<int> notFullRange = invent.findAllIndexesNotFullItemsOfType(canvas.selectedSlot.GetItem().type);
        if (notFullRange != null)
        {
            foreach (int i in notFullRange)
            {
                if (selectedSlots == null) selectedSlots = new List<int>();
                 if (!selectedSlots.Contains(i)) selectedSlots.Add(i);
            }
        }
        
        if (selectedSlots.Contains(sel) && selectedSlots != null) selectedSlots.Remove(sel);

        if(selectedSlots != null)
        {
            foreach (int i in selectedSlots)
            {
                manager.playerInvs.slots[i].highLight();
            }
        }
        //*/
    }
    public void unselectAll()
    {
        foreach (InvSlot slot in slots)
        {
            if (slot.highLightIcon.activeSelf)
            {
                slot.highLight(false);
            }
        }
    }

    public void activateBackpackSlots(bool act=true)
    {
        if (num == 0)
        {
            for (int i = 8; i < 12; i++)
            {
                InvSlot newSlot = Instantiate(slotPrefab, this.transform).GetComponent<InvSlot>();
                slots.Add(newSlot);
                slots[i].transform.parent = this.transform;
            }
        }
        
    }
}
