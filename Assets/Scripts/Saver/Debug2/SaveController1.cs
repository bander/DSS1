using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveController1 : MonoBehaviour
{
    #region Singleton
    public static SaveController1 instance;

    void Awake()
    {
        instance = this;
        LoadData();
    }
    #endregion

    public Item[] prefabs;

    public SaveInv saveData;
    [ContextMenu("Save Data")]
    public void SaveGame()
    {
        var data = JsonUtility.ToJson(saveData);
        Debug.Log("SAAAAVE " + data);
        PlayerPrefs.SetString("GameData3", data);
    }
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        var data = PlayerPrefs.GetString("GameData3");
        Debug.Log("LOOOAD "+ data);
        saveData = JsonUtility.FromJson<SaveInv>(data);
    }
    private void OnDisable()
    {
        SaveGame();
    }


    public void SetItem(Vector3 v, int n)
    {
        /*
        if (saveData.items == null) { 
            saveData.items = new List<Vector3>(8);
        }
        if (saveData.items.Count<8)
        {
            makeEmptyDate();
        }
        saveData.items[n] = v;
        SaveGame();
        //*/
    }
    public void SetItems(List<Item> _items)
    {
        if (saveData.items == null)
        {
            saveData.items = new List<Vector3>(8);
        }
        if (saveData.items.Count < 8)
        {
            makeEmptyData();
        }
        int i = 0;
        foreach (Item it in _items)
        {
            int num=0;
            int cnt=0;
            if (it != null)
            {
                num = GetNumOfItem(it);
                cnt = it.countInSlot;
            }
            saveData.items[i] = new Vector3(num,cnt,0);
            Debug.Log("From Inv " +i+"  (x="+num+" y="+cnt+")");
            i++;
        }
        SaveGame();
    }
    public void SetItems2(List<Item> _items)
    {
        if (saveData.items2 == null)
        {
            saveData.items2 = new List<Vector3>(8);
        }
        if (saveData.items2.Count < 8)
        {
            makeEmptyData_2();
        }
        int i = 0;
        foreach (Item it in _items)
        {
            int num = 0;
            int cnt = 0;
            if (it != null)
            {
                num = GetNumOfItem(it);
                cnt = it.countInSlot;
            }
            saveData.items2[i] = new Vector3(num, cnt, 0);
            Debug.Log("From 222 Inv " + i + "  (x=" + num + " y=" + cnt + ")");
            i++;
        }
        SaveGame();
    }

    public List<Item> GetItems()
    {
        Debug.Log("GET");
        List<Item> itms = new List<Item>(8);
        makeEmptyData2(itms);
        int i = 0;
        foreach (Vector3 v in saveData.items)
        {
            if (v.x == 0)   
            {
                itms[i] = null;
            }
            else
            {
                itms[i] = prefabs[(int)v.x].Clone();
            }
            if (v.x!=0 && v.y > 0) itms[i].countInSlot = (int)v.y;


            i++;
        }
        return itms;
    }
    public List<Item> GetItems2()
    {
        Debug.Log("GET 2");
        List<Item> itms = new List<Item>(8);
        makeEmptyData2(itms);
        int i = 0;
        foreach (Vector3 v in saveData.items2)
        {
            if (v.x == 0)
            {
                itms[i] = null;
            }
            else
            {
                itms[i] = prefabs[(int)v.x].Clone();
            }
            if (v.x != 0 && v.y > 0) itms[i].countInSlot = (int)v.y;
            i++;
        }
        return itms;
    }

    public void makeEmptyData()
    {
        while (saveData.items.Count < 8)
        {
            saveData.items.Add(new Vector3(0,0,0));
        }
    }
     public void makeEmptyData_2()
    {
        while(saveData.items2.Count<8)
        {
            saveData.items2.Add(new Vector3(0,0,0));
        }
    }
    public void makeEmptyData2(List<Item> itm)
    {
        while(itm.Count<8)
        {
            itm.Add(null);
        }
    }
    int GetNumOfItem(Item _item)
    {
        int i = 0;
        foreach (Item item in prefabs)
        {
            if (i > 0)
            {
                Debug.Log(i+" "+ item.name+" //  "+_item.name);
                if (item.name == _item.name)
                {
                    return i;
                }

            }
            i++;
        }
        return 0;
    }
}