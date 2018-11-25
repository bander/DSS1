using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventory : Interactable
{
    [SerializeField]
    public List<Item> items;

    Invent invent;
    public int loadedNumber;

    void Start()
    {
        player = PlayerInteractions.instance;

        gameObject.AddComponent<Invent>();
        invent = GetComponent<Invent>();
        invent.Init(8);
        
  //      items = SaveController1.instance.GetItems2();
        foreach (Item item in items)
        {
            invent.Add(item);
        }
    }

    public override void Interact()
    {
        base.Interact();

        InvManager manager = InvManager.instance;
        manager.invents[2] = invent;
        
        
        if (manager.OnInvChangedCallback != null)
            manager.OnInvChangedCallback.Invoke();

        CanvasController.instance.ShowLoot();
    }

    public void AddDiscoveredItem(Item itm)
    {
        invent.Add(itm);
    }
}
