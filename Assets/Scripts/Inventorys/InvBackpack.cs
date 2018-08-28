using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvBackpack : Invent {
    InvManager manager;
    void Start()
    {
        manager = InvManager.instance;
    }

    public override bool isEmptySlotAvailable()
    {
        int i = 0;
        foreach (Item item in items)
        {
            Debug.Log(i+"  "+item);
            i++;
            if (item == null)  return true;
        }
        return false;
    }
    
}
