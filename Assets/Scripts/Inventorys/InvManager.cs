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

        //trashPrefab.countInSlot = 16;
        // invents[0].Add(trashPrefab);

        //SaveInventory.Load();
        //Debug.Log("load "+SaveInventory.Instance.items.Count);
        /* if (SaveInventory.Instance.items.Count>0)
         {
             Debug.Log("load " + SaveInventory.Instance.items[0]);
         }
         //*/
        // invents[0].Add(trashPrefab);
        //invents[0].InitLoading(SaveInventory.Instance.items);
        /*
        if (OnInvChangedCallback != null)
            OnInvChangedCallback.Invoke();

        AddToInventory(trashPrefab, 0);
        AddToInventory(trashPrefab2, 0);
        SaveInventory.Instance.items.Add(new Vector3(0, 0, Random.Range(2, 7)));// =invents[0].items;
        SaveInventory.Save();

        //*/


        /*
        invents[0].items = GameDataController.GetItems2();
        if (OnInvChangedCallback != null)
            OnInvChangedCallback.Invoke();

        //*/
//        invents[0].items = SaveController1.instance.GetItems();
        if (OnInvChangedCallback != null)
            OnInvChangedCallback.Invoke();
    }
    public Item trashPrefab;
    public Item trashPrefab2;
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
    

    public bool AddToInventory(Item item,int numInv)
    {
        if (numInv==0 && item!=null)
        {
            if (invents[0].isEmptySlotAvailable())
            {
                invents[0].Add(item);

                if (OnInvChangedCallback != null)
                    OnInvChangedCallback.Invoke();

                return true;
            }else if (item.maxCountInSlot > 1)
            {
                if (invents[0].findIndexNotFullItemOfType(item.type) != -1)
                {
                    invents[0].Add(item);
                }
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
                    ///ищем все незаполненные слоты с аналогичным айтемом
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

    public Equipment GetCurrentWeapon()
    {
        Equipment eq=null;
        int weaponIndex=-1;

        int i = 0;
        if (invents.Count > 1 && invents[1] != null)
        {
            foreach (Item item in invents[1].items)
            {
                if ((item as Equipment) != null)
                {
                    if ((item as Equipment).equipSlot == EquipmentSlot.Weap)
                    {
                        weaponIndex = i;
                        break;
                    }
                }
                i++;
            }
            weaponIndex = 1;
            if (weaponIndex != -1)
            {
                eq = invents[1].items[weaponIndex] as Equipment;
            }

        }

        return eq;
    }
    public Item GetFastSlotItem()
    {
        Item itm = null;
        int fastIndex = -1;

        if (invents.Count>0 && invents[1]!=null)
        {
            int i = 0;
            foreach (Item item in invents[1].items)
            {
                if (item!= null)
                {
                    if (item.equipSlot2 == EquipmentSlot.Slot)
                    {
                        fastIndex = i;
                        //Debug.Log(" ff "+i+"  __ "+item);
                        break;
                    }
                }
                i++;
            }
            //////   
            ///      Проверка конкретного слота инвенторя без обхода
            fastIndex = 5;
            if (fastIndex != -1)
            {
                itm = invents[1].items[fastIndex] as Equipment;
            }
        }
        

        return itm;
    }

    public void SaveAllInventories()
    {
//        SaveController1.instance.SetItems(invents[0].items);
//        SaveController1.instance.SetItems2(invents[2].items);
        //GameDataController.SetItems(invents[0].items);
    }

    public bool HasPickInBackpack()
    {
        foreach (Item item in invents[0].items)
        {
            if (item!=null && item.name == "Picker")
            {
                return true;
            }
        }
        return false;
    }
}