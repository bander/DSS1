using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {
    public delegate void OnHit();
    public OnHit onHit;

    public delegate void OnUpdate();
    public OnUpdate onUpdate;

    public Transform player;
   

    void Start()
    {
        player = PlayerManager.instance.player.transform;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (onHit != null) onHit.Invoke();

        //if(onUpdate==null && !dead) onUpdate += rotateToPlayer;
    }
    /*void Update()
    {
        if (onUpdate != null) onUpdate.Invoke();
    }
    void rotateToPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(player.position-transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 250);
    }
    //*/

    public override void Die()
    {
        if (!dead)
        {
            base.Die();
            onUpdate = null;
            onHit = null;

//            GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuScript>().RemoveEnemy(GetComponent<Interactable>());
             GetComponentInChildren<Animator>().SetTrigger("Die");

            MakeLoot();
            //            MenuScript.instance.RemoveEnemy(GetComponent<EnemySample>());

            PlayerManager.instance.player.GetComponent<PlayerControl>().CheckCurerntTarget();
        }
    }

    void MakeLoot()
    {
        LootInventory loot = GetComponent<LootInventory>();
        if(loot!=null) loot.enabled = true;
    }
}
