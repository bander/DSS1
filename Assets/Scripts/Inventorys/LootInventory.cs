using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventory : Interactable
{
    [SerializeField]
    Item[] items;

    Invent invent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        gameObject.AddComponent<Invent>();
        invent = GetComponent<Invent>();
        invent.Init(8);
        int i = 0;
        foreach (Item item in items)
        {
            invent.Add(item);
        }

    }

    public override void Interact()
    {
        base.Interact();
        openLootMenu();
    }

    void openLootMenu()
    {
        InvManager manager = InvManager.instance;
        manager.invents[2] = invent;
        
        
        if (manager.OnInvChangedCallback != null)
            manager.OnInvChangedCallback.Invoke();

        CanvasController.instance.ShowLoot();

    }
}
