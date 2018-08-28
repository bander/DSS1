using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour {
    EquipmentManager manager;

    public InventorySlot[] slots;
    public Dictionary<EquipmentSlot, InventorySlot> links = new Dictionary<EquipmentSlot, InventorySlot>();
   

	void Start () {
        manager = EquipmentManager.instance;
        manager.OnEquipChangedCallback += UpdateEquipment;

        foreach (InventorySlot s in slots)
        {
            if (s.equipmentSlot != EquipmentSlot.None)
            {
                links.Add(s.equipmentSlot, s);
                Debug.Log("add link " + s.equipmentSlot);
            }
        }
    }
	
    void UpdateEquipment()
    {
        Equipment[] es = manager.currentEquipment;
        Debug.Log("Update !!! "+ manager.currentEquipment.Length+"  //  ");
        foreach (Equipment  e in es)
        {
            if (e!=null)
            {
                if (e.equipSlot != EquipmentSlot.None)
                {
                    Debug.Log(" present " + e.equipSlot);
                    links[e.equipSlot].AddItem(e);
                }
            }
        }
    }
}
