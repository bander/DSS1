using UnityEngine;

public class InventoryUI : MonoBehaviour {
    public GameObject inventoryUI;
    public Transform itemsParent;

    
    Inventory inventory;

    InventorySlot[] slots;

	void Start () {
        inventory = Inventory.instance;
        inventory.OnItemChangedCallback += UpdateUI;

 //       slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}
	
    void UpdateUI()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (i<inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
