using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item")]
public class Item : ScriptableObject{
    new public string name = "New Item";
    public SkinnedMeshRenderer mesh;

    public Sprite icon = null;
    public bool isDefaultItem = false;
    public int maxCountInSlot=1;

    public virtual void Use()
    {
        
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
