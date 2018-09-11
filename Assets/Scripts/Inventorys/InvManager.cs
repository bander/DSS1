using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvManager : MonoBehaviour {
    #region Singleton
    public static InvManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public Invs playerInvs;
    public Invs EquipInvs;
    public Invs lootInvs;


    public Item tm;
    public Item tm2;

    public List<Invent> invents = new List<Invent>();


    public delegate void OnInvChanged();
    public OnInvChanged OnInvChangedCallback;

    void Start()
    {
        GameObject invent= new GameObject();
        invent.AddComponent<InvBackpack>();
        invent.GetComponent<InvBackpack>().Init(8);
        invents.Add(invent.GetComponent<Invent>());

        GameObject invent2 = new GameObject();
        invent2.AddComponent<Invent>();
        invent2.GetComponent<Invent>().Init(7);
        invents.Add(invent2.GetComponent<Invent>());

        GameObject invent3 = new GameObject();
        invent3.AddComponent<Invent>();
        invent3.GetComponent<Invent>().Init(8);
        invents.Add(invent3.GetComponent<Invent>());
        
        
        playerInvs.SetNum(0);
        EquipInvs.SetNum(1);
        lootInvs.SetNum(2);
    }
    public void SetBackpack(bool act = true)
    {
        if (act)
        {
            playerInvs.activateBackpackSlots();
            invents[0].Init(4);
        }
        else
        {
            playerInvs.activateBackpackSlots(false);
            invents[0].reinit(4);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tm.countInSlot = 12;
            invents[0].Add(tm.Clone());

            if (OnInvChangedCallback != null)
                OnInvChangedCallback.Invoke();
        }
    }

    public bool AddToInventory(Item item,int numInv)
    {

        if (numInv==0)
        {
            if (invents[0].isEmptySlotAvailable())
            {
                invents[0].Add(item);

                if (OnInvChangedCallback != null)
                    OnInvChangedCallback.Invoke();

                return true;
            }else if(item.maxCountInSlot>1){
                invents[0].findIndexNotFullItemOfType(item.type);
            }
     
            return false;
        }
        return false;
    }

    public Invent GetInvent(int num)
    {
        if (invents.Count>num)
        {
            return invents[num];
        }
        return null;
    }

    public bool IsLootEmpty()
    {
        if(invents!=null && invents.Count>1 && invents[2]!=null && invents[2].items != null)
        {
            foreach (Item item in invents[2].items)
            {
                if (item != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void GetAllLoot()
    {


        List<Item> loot = invents[2].items;
        int j = 0;

        int i = 0;
        foreach (Item item in loot)
        {
            if (item != null)
            {
                int indexEmpty;
                if (item.maxCountInSlot < 1)
                {
                    indexEmpty = invents[0].findIndexEmptySlot();
                
                    if (indexEmpty != -1)
                    {
                        invents[2].SwitchItems(i, indexEmpty, 2, 0);
                    }
                }
                else
                {
                    ///мщес все слот незаполненнык с аналогичным айтемом
                    ///заполняем его
                    ///если не все айтемы перенесены
                    ///ищем следующий аналогичный незаполненный слот
                    
                    bool stillSearchAnalogItems = true;
                    while (stillSearchAnalogItems && invents[2].items[i]!= null)
                    {
                        int indexNotFull = invents[0].findIndexNotFullItemOfType(item.type);
            
                        if (indexNotFull != -1)
                        {
                            invents[2].fillItems(i,indexNotFull, 2, 0);

                            if (invents[2].items[i] == null)
                            {
                                stillSearchAnalogItems = false;
                            }
                            else if(invents[2].items[i].countInSlot == 0)
                            {
                                stillSearchAnalogItems = false;
                            }
                        }
                        else
                        {
                            stillSearchAnalogItems = false;
                        }
                    }

                    /// если перенесли не все элементы 
                    /// проверяем есть ли пустой слот

                    if (invents[2].items[i] != null)
                    {
                        indexEmpty = invents[0].findIndexEmptySlot();
                        if (indexEmpty != -1)
                        {
                            invents[2].SwitchItems(i, indexEmpty, 2, 0);
                        }
                    }
                        
                }
            }
            i++;

        }

        OnInvChangedCallback.Invoke();
    }

    public void spendItems(Item.ItemType inType,int count)
    {
        if (invents[0].GetSummMetall() >= count)
        {
            while (count > 0)
            {
                int i = indexMinCountOfType(inType);
                spendOneItemFromIndex(i);
                count--;
            }
        }
    }

    int indexMinCountOfType(Item.ItemType inType)
    {
        int min = 1000;
        int index=-1;
        int i = 0;
        foreach (Item item in invents[0].items)
        {
            if (item != null)
            {
                if (item.type == inType)
                {
                    if (item.countInSlot < min)
                    {
                        min = item.countInSlot;
                        index = i;
                    }
                }
            }
            i++;
        }
        return index;
    }
    void spendOneItemFromIndex(int index)
    {
        if (invents[0].items[index].countInSlot > 1)
        {
            invents[0].items[index].countInSlot -= 1;
        }
        else
        {
            invents[0].items[index] = null;
        }

        OnInvChangedCallback.Invoke();
    }
}