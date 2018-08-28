using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invs : MonoBehaviour {
    InvManager manager;
    [SerializeField]
    InvSlot[] slots;
    int num = 0;

    void Start()
    {
        manager = InvManager.instance;

        if (slots == null)
        {
            slots = GetComponentsInChildren<InvSlot>();
        }
    }
    public void SetNum(int number)
    {
        if (manager == null)
        {
            manager = InvManager.instance;
        }
        num = number;
        manager.OnInvChangedCallback += UpdateSlots;

        UpdateSlots();
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
                        s.SetItem(tm, i,num);
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


}
