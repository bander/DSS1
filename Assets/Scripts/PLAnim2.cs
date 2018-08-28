using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAnim2 : PlayerAnimator {
    public WeaponAnimations[] weaponAnimations;
    Dictionary<Equipment, AnimationClip[]> weaponAnimationDict;
    protected override void Start()
    {
        base.Start();
        EquipmentManager.instance.onEquipmentChange += OnEquipmmentChange;

        weaponAnimationDict = new Dictionary<Equipment, AnimationClip[]>();
        foreach (WeaponAnimations a in weaponAnimations)
        {
            weaponAnimationDict.Add(a.weapon, a.clips);
        }
    }
    void OnEquipmmentChange(Equipment newItem,Equipment oldItem)
    {

        /*
        if (oldItem != null && oldItem.equipSlot == EquipmentSlot.Weapon)
        {
            animator.SetLayerWeight(1, 0);
            currentAttackAnimSet = defaultAttackAnimSet;
        }

        if (oldItem != null && oldItem.equipSlot == EquipmentSlot.Shield)
        {
            animator.SetLayerWeight(2, 0);
        }
        if (newItem != null && newItem.equipSlot == EquipmentSlot.Shield)
        {
            animator.SetLayerWeight(2, 1);
        }

        if (newItem!=null && newItem.equipSlot == EquipmentSlot.Weapon)
        {
            animator.SetLayerWeight(1, 1);

            if (weaponAnimationDict.ContainsKey(newItem))
            {
                currentAttackAnimSet = weaponAnimationDict[newItem];
            }
        }
        //*/
    }

    [System.Serializable]
    public struct WeaponAnimations
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
