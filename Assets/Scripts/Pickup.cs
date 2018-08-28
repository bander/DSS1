using UnityEngine;

public class Pickup : Interactable {
    public Item item;

    void Start()
    {
        radius = 1.5f;
    }
    public override void Interact()
    {
        base.Interact();

        GetItem();
    }
    void GetItem()
    {
        bool wasPickuped = Inventory.instance.Add(item);
        if(wasPickuped) Destroy(gameObject);
    }
}
