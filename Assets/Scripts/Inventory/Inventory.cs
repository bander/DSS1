using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton
    public static Inventory instance;
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one inventory found");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;

    public int space = 20;

    public List<Item> items = new List<Item>(16);

    void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            items.Add(null);
        }
    }

    public bool Add(Item item)
    {
        if (!item.isUsable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    if (OnItemChangedCallback != null)
                    OnItemChangedCallback();
                    return true;
                }
            }
            return false;
        }
        return false;
    }   
    public void Remove(Item item)
    {
        items.Remove(item);

        if (OnItemChangedCallback != null)
            OnItemChangedCallback();
    }
}
