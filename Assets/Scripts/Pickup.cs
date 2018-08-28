using UnityEngine;

public class Pickup : Interactable {
    public Item item;
    
    public override void Interact()
    {
        base.Interact();

        GetItem();
    }
    void GetItem()
    {
        //bool wasPickuped = Inventory.instance.Add(item);

        bool wasPickuped = InvManager.instance.AddToInventory(item,0);
        if(wasPickuped) Destroy(gameObject);
    }
}
