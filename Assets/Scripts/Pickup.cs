using UnityEngine;

public class Pickup : Interactable {
    public Item item;
    
    public override void Interact()
    {
        base.Interact();
        
        //bool wasPickuped = Inventory.instance.Add(item);
        bool wasPickuped = InvManager.instance.AddToInventory(item,0);
        if (wasPickuped)
        {
            //            MenuScript.instance.removeImteractables(this);
            player.removeInteractables(this);
            Destroy(gameObject);
        }
    }
}