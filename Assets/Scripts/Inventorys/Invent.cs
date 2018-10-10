using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invent : MonoBehaviour {

    [SerializeField]
    public List<Item> items = new List<Item>();

    InvManager manager;

    public void Init(int count)
    {
        manager = InvManager.instance;
        for (int i = 0; i < count; i++)
        {
            items.Add(null);
        }
    }
    public void reinit(int count)
    {
        int i = 0;
        while (i < count)
        {
            items.RemoveAt(items.Count - 1);
            i++;
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

    public int findIndexEmptySlot()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) return i;
        }
        return -1;
    }
    public List<int> findAllIndexesEmptySlots()
    {
        List<int> ret = new List<int>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) ret.Add(i);
        }
        if (ret.Count == 0)  ret=null;
        return ret;
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
    public List<int> findAllIndexesNotFullItemsOfType(Item.ItemType findType)
    {
        List<int> ret = new List<int>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].type == findType && items[i].countInSlot < items[i].maxCountInSlot)
            {
                ret.Add(i);
            }
        }
        if (ret.Count == 0) ret = null;
        return ret;
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
        if(i>=0 && items.Count>i) items[i] = null;

        if (InvManager.instance.OnInvChangedCallback!=null)
        {
            InvManager.instance.OnInvChangedCallback.Invoke();
        }
    }
    public void RemoveItemByNum(int i)
    {
        if (i >= 0 && items.Count > i) items[i] = null;
        InvManager.instance.OnInvChangedCallback.Invoke();
    }

    public void SwitchItems(int from,int to, int fromInv,int toInv)
    {
        Item switchedItem;

        if (fromInv == toInv)
        {
            switchedItem = items[to];
            items[to] = items[from];
            items[from] = switchedItem;
        }
        else
        {
            Invent otherInvent = InvManager.instance.GetInvent(toInv);//.Add(items[from]);

            switchedItem = otherInvent.items[to];
            otherInvent.items[to] = items[from];
            items[from] = switchedItem;

            if(toInv==1 && (otherInvent.items[to] as Equipment).equipSlot == EquipmentSlot.Back)
            {
                manager.SetBackpack();
                return;
            }
            if (fromInv == 1)
            {
                if ((otherInvent.items[to] as Equipment).equipSlot == EquipmentSlot.Back)
                {
                    manager.SetBackpack(false);
                    return;
                }
            }
        }
    }
    public void fillItems(int from, int to, int fromInv, int toInv)
    {
        if (fromInv == toInv)
        {
            int availableCount = items[to].maxCountInSlot-items[to].countInSlot;
            if (items[from].countInSlot > availableCount)
            {
                items[to].countInSlot = items[to].maxCountInSlot;
                items[from].countInSlot -= availableCount;
            }
            else
            {
                items[to].countInSlot += items[from].countInSlot;
                items[from] = null;
            }
        }
        else
        {
            Invent otherInvent = InvManager.instance.GetInvent(toInv);//.Add(items[from]);
            
            int availableCount = otherInvent.items[to].maxCountInSlot - otherInvent.items[to].countInSlot;
         
            if (items[from].countInSlot > availableCount)
            {
                otherInvent.items[to].countInSlot = otherInvent.items[to].maxCountInSlot;
                items[from].countInSlot -= availableCount;
            }
            else
            {
                otherInvent.items[to].countInSlot += items[from].countInSlot;
                items[from] = null;
            }
        }
    }
    public void UpdateCountOfItems(Item newItem,int num,int count)
    {
        if (count == 0)
        {
            items[num] = null;
        }
        else
        {
            if (items[num] == null) items[num] = newItem.Clone();
            items[num].countInSlot = count;
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
    public int GetSummMetall()
    {
        int ret = 0;
        foreach (Item item in items)
        {
            if (item != null)
            {
                if (item.type == Item.ItemType.Metall)
                {
                    ret += item.countInSlot;
                }
            }
        }
        return ret;
    }
}
