using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
        bool ret = false;
        int i = -1;
        if (newItem.countInSlot ==1)
        {
            i = findIndexEmptySlot();
            if(i != -1) items[i] = newItem.Clone();
        }
        else
        {
            int excess = fillSlotByType(newItem.Clone());
            if (excess != 0)
            {
                i = findIndexEmptySlot();
                if (i != -1)
                {
                    Item excessItem = newItem.Clone();
                    excessItem.countInSlot = excess;
                    items[i] = excessItem;
                }
            }
            else
            {
                i = 0;
            }
        }
        if (i != -1) ret = true;


        return ret;
    }
    int findIndexEmptySlot()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) return i;
        }
        return -1;
    }
    public int findIndexNotFullItemOfType(Item.ItemType findType)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].type == findType && items[i].countInSlot < items[i].maxCountInSlot)
            {
                return i;
            }
        }
        return -1;
    }
    int fillSlotByType(Item fillItem)
    {
        int fillCount = fillItem.countInSlot;
        int maxCount = fillItem.maxCountInSlot;
        int i = findIndexNotFullItemOfType(fillItem.type);

        while (i != -1)
        {
            int emptyCount = maxCount - items[i].countInSlot;

            if (emptyCount >= fillCount)
            {
                items[i].countInSlot += fillCount;
                fillCount = 0;

                i = -1;
            }
            else
            {
                items[i].countInSlot = maxCount;
                fillCount -= emptyCount;
                i = findIndexNotFullItemOfType(fillItem.type);
            }
        }

        return fillCount;
    }
    
    public void Remove(Item item)
    {
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
        int i = 0;
        foreach (Item item in items)
        {
            i++;
            if (item == null) return true;
        }
        return false;
    }
}
