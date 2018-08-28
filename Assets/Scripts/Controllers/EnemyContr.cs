using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContr : MonoBehaviour {

    Transform target;
    NavMeshAgent agent;
    Enemy me;

    CharacterCombat combat;

    public float lookRadius=10f;

	// Use this for initialization
	void Start () {
        me = GetComponent<Enemy>();

        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;

        combat = GetComponent<CharacterCombat>();
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(target.position, transform.position);
       
        if (distance < lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance<agent.stoppingDistance)
            {
                FaceToTarget();

                ////////////////////
                me.Interact();
                //////////////

                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                if (targetStats != null)
                {
                    combat.Attack(targetStats);
                }
                

            }
        }
	}

    void FaceToTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = lookRotation;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
