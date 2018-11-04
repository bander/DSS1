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

        List<Item> loaded=new List<Item>();
        if (SaveGameInventory.Instance.itemsLoot1.Count>0)
            loaded = SaveGameInventory.Instance.itemsLoot1;

        if (loaded.Count!=0)
        {
            items = loaded;
        }
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
}
