using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Equipment",menuName="Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlot equipSlot;
    public WeaponAttackType attackType;

    public int armorModifier;
    public int damageModifier;
    public float attackDistance;
    public float attackRate;
    public float speed;

    public AudioClip audioClip;
    public GameObject muzzleEffect;
    public GameObject bullet;

    public override void Use()
    {
        base.Use();

        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
    public override Item Clone()
    {
        Equipment itm = new Equipment();
        itm.name = name;
        itm.mesh = mesh;

        itm.icon = icon;
        itm.isDefaultItem = isDefaultItem;
        itm.countInSlot = countInSlot;
        itm.maxCountInSlot = maxCountInSlot;
        itm.type = type;
        itm.equipSlot = equipSlot;
        itm.equipSlot2 = equipSlot2;
        itm.attackType = attackType;
        itm.armorModifier = armorModifier;
        itm.damageModifier = damageModifier;

        itm.attackDistance = attackDistance;
        itm.attackRate = attackRate;
        itm.speed = speed;
        itm.audioClip = audioClip;
        itm.muzzleEffect = muzzleEffect;
        itm.bullet = bullet;

        return itm;
    }
}

public enum EquipmentSlot { None,Head,Body,Legs,Foot,Weap,Back,Slot }
public enum WeaponAttackType { None,Sword,Pistol,Automative }