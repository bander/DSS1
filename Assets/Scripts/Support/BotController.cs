using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour {
    [SerializeField] Item prefab;

    Animator anim;
    NavMeshAgent agent;
    Vector3 currentDestination;
    LootInventory loot;
    bool isDiscovering;
    float discoverTimer;
    float discoverTime=5;

	void Start () {
        loot = GetComponent<LootInventory>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //agent.SetDestination(new Vector3(-11,0,-40));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isDiscovering = true;
            anim.SetBool("Discover", true);
            anim.SetBool("Forward", false);
        }
        if (!isDiscovering) return;

        discoverTimer += Time.deltaTime;
        if (discoverTimer > discoverTime)
        {
            discoverTimer = 0;
            _DiscoverComplete();
        }
    }
    
	void _DiscoverComplete()
    {
        GetComponent<LootInventory>().AddDiscoveredItem(prefab.Clone());
        anim.SetBool("Discover", false);
        anim.SetBool("Forward", true);
        isDiscovering = false;
    }
}
