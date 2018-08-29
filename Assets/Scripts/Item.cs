using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item")]
public class Item : ScriptableObject{
    new public string name = "New Item";
    public SkinnedMeshRenderer mesh;

    public Sprite icon = null;
    public bool isDefaultItem = false;
    //[HideInInspector]
    public int countInSlot = 1;
    public int maxCountInSlot=1;
    public ItemType type;

    public enum ItemType {Equip,Metall,Oxygen };

    public virtual void Use()
    {
        
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }

    public virtual Item Clone()
    {
        Item itm = new Item();
        itm.name = "New Item";
        itm.mesh = mesh;

        itm.icon = icon;
        itm.isDefaultItem = isDefaultItem;
        itm.countInSlot = countInSlot;
        itm.maxCountInSlot = maxCountInSlot;
        itm.type=type;

        return itm;
    }
}
