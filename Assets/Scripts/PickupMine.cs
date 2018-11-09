using UnityEngine;

public class PickupMine : Interactable {
    public Item item;
    public int iterations=3;
    
    public override void Interact()
    {
        base.Interact();

        iterations--;
        if (iterations >=0) return;
        //bool wasPickuped = Inventory.instance.Add(item);
        bool wasPickuped = InvManager.instance.AddToInventory(item,0);
        
            //MenuScript.instance.removeImteractables(this);
            player.removeInteractables(this);
            Destroy(gameObject);
    }
}