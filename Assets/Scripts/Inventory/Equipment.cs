using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Equipment",menuName="Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlot equipSlot;

    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();

        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
    public override Item Clone()
    {
        Equipment itm = new Equipment();
        itm.name = "New Item";
        itm.mesh = mesh;

        itm.icon = icon;
        itm.isDefaultItem = isDefaultItem;
        itm.countInSlot = countInSlot;
        itm.maxCountInSlot = maxCountInSlot;
        itm.type = type;


        itm.equipSlot = equipSlot;

        return itm;
    }
}

public enum EquipmentSlot { None,Head,Body,Legs,Foot,Weap,Back,Slot }