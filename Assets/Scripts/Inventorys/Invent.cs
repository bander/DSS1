using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invent : MonoBehaviour {
    public List<Item> items = new List<Item>();

    public void Init(int count)
    {
        for (int i = 0; i < count; i++)
        {
            items.Add(null);
        }
    }
    public bool Add(Item newItem)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                return true;
            }
        }
        return false;
        
    }
    public void Remove(Item item)
    {
        //items.Remove(item);
        int i = items.IndexOf(item);
        items[i] = null;
    }

    public void SwitchItems(int from,int to, int fromInv,int toInv)
    {
        if (fromInv == toInv)
        {
            items[to] = items[from];
            items[from] = null;
        }
        else
        {
            Invent otherInvent = InvManager.instance.GetInvent(toInv);//.Add(items[from]);
            otherInvent.items[to] = items[from];
            Remove(items[from]);
        }
    }

    public virtual bool isEmptySlotAvailable()
    {
        Debug.Log("EMPTY AV");
        return true;
    }
}
