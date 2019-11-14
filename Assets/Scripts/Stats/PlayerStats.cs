using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {

    public float maxOxygen = 100;
    public float currentOxygen;// { get; private set; }
    public float maxEnergy = 100;
    public float currentEnergy;// { get; private set; }

    public delegate void OnHPChange();
    public OnHPChange onHPChange;

    public delegate void OnStatsUpdate();
    public OnStatsUpdate onStatsUpdate;
    int weaponNum=-1;
    Equipment weapon;
    public int WepaonNum { get { return weaponNum; } }
    public Equipment Wepaon { get { return weapon; } }

    List<Equipment> currentEquipment=new List<Equipment>();

    public bool pistolInHand = false;

    void Start () {
        for (int i = 0; i < 7; i++)
        {
            currentEquipment.Add(null);
        }
        InvManager.instance.OnInvChangedCallback += checkAllEquipmentSlots;

        currentEnergy = 100;
        currentOxygen = 50;
	}
    void checkAllEquipmentSlots()
    {
        bool equipmentWasChanged = false;
        Invent invent = InvManager.instance.GetInvent(1);
        int i = 0;
        foreach (Item item in invent.items)
        {
            if (i != 5)
            {
                Equipment eq = item as Equipment;
                if (currentEquipment[i] != eq)
                {
                    onEquipmentChange(eq, currentEquipment[i]);
                    currentEquipment[i] = eq;
                    equipmentWasChanged = true;

                    if (i == 1)
                    {
                        if (eq != null)
                        {
                            weaponNum = (int)eq.attackType;
                            weapon = eq;
                        }
                        else
                        {
                            weaponNum = -1;// (int)eq.attackType;
                            weapon = null;
                        }
                    }
                }
            }
            i++;
        }
        if(equipmentWasChanged && onStatsUpdate != null) onStatsUpdate.Invoke();

        if(equipmentWasChanged)
            TestEnemySpawn();
    }
	
    void onEquipmentChange(Equipment newItem,Equipment oldItem)
    {
        if (newItem != null) {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            attackRate.AddModifier(newItem.attackRate);
            attackDist.AddModifier(newItem.attackDistance);
            speed.AddModifier(newItem.speed);
        }
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            attackRate.RemoveModifier(oldItem.attackRate);
            attackDist.RemoveModifier(oldItem.attackDistance);
            speed.RemoveModifier(oldItem.speed);
        }
    }
    private void TestEnemySpawn()
    {
        //GameObject.FindObjectOfType<TestEnemySpawner>().ActivateSpawn(weaponNum>1);
        pistolInHand = (weaponNum > 1);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //if (onHPChange != null) onHPChange.Invoke();
        
        if (PlayerStatsBars.instance == null) return;

        PlayerStatsBars.instance.UpdateStats();
    }
    public void ChangeOxygenBy(int n)
    {
        currentOxygen -= n;
        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
        }

        if (PlayerStatsBars.instance == null) return;

        PlayerStatsBars.instance.UpdateStats();
        //SaveStats();
    }
    public void ChangeEnergyBy(int n)
    {
        currentEnergy -= n;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
        if (PlayerStatsBars.instance == null) return;
        PlayerStatsBars.instance.UpdateStats();
        //SaveStats();
    }

    public override void Die()
    {
        base.Die();
        PlayerManager.instance.KillPlayer();
    }

    void SaveStats()
    {
        Vector3 s = new Vector3(currentHealth,currentOxygen,currentEnergy);
        SaveGame.Instance.s=s;
        SaveGame.Save();
    }
}
