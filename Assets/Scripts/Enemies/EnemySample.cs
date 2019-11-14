using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySample : Interactable {

    public delegate void OnEnemyHPChange();
    public OnEnemyHPChange onEnemyHPChange;

    public string enemyName = "Enemy1";
    
    public override void inRange()
    {
        if (isInRange)
        {
            if (!stats.dead)
            { //MenuScript.instance.AddEnemy(this);
                player.AddEnemy(this);
                if(GetComponent<EnemyMovement>()!=null)//.StartFollow();
                   GetComponent<EnemyMovement>().StartFollow();
                if (GetComponent<EnemySerpentMovement>() != null)//.StartFollow();
                    GetComponent<EnemySerpentMovement>().StartFollow();
            }
        }
        else
        {
            player.RemoveEnemy(this);
            //MenuScript.instance.RemoveEnemy(this);
        }
    }

}
