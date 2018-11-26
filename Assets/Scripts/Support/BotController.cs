using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour {
    [SerializeField] Item prefab;
    [SerializeField] GameObject[] destinations;
    [SerializeField] float stoppingDistForPlayer;

    Animator anim;
    NavMeshAgent agent;
    int currentDestNumber;
    Vector3 currentDestination;
    LootInventory loot;
    bool isDiscovering;
    float discoverTimer;
    [SerializeField] float discoverTime =5;

    bool nearPlayer=false;
    bool isMoving=false;
    Transform player;

	void Start () {
        destinations = GameObject.FindGameObjectsWithTag("BotDestinations");
        loot = GetComponent<LootInventory>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        player = PlayerManager.instance.player.transform;
        nextDestination();
        //agent.SetDestination(new Vector3(-11,0,-40));
    }

    void Update()
    {
        if (Vector3.Distance(transform.position,player.position)<stoppingDistForPlayer)
        {
            if (!nearPlayer)
            {
                nearPlayer = true;
                WaitForPlayer();
            }
        }
        else
        {
            if (nearPlayer)
            {
                nearPlayer = false;
                WaitForPlayer(false);
            }
        }

        if (!nearPlayer && isMoving) MoveByNavigation();

        if (!isDiscovering) return;

        discoverTimer += Time.deltaTime;
        if (discoverTimer > discoverTime)
        {
            discoverTimer = 0;
            _DiscoverComplete();
        }
    }
    void MoveByNavigation()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        if (!AgentDone())
        {
            velocity = agent.desiredVelocity;// Quaternion.Inverse(transform.rotation) * agent.desiredVelocity;
            velocity.y = 0;
            if (Quaternion.LookRotation(velocity) != null) transform.rotation = Quaternion.LookRotation(velocity);
            
        }
        else
        {
            StartDiscover();
        }
        anim.SetBool("Forward", (velocity.magnitude > 0.00001f));
    }
    protected bool AgentDone()
    {
        return AgentStopping() && !agent.pathPending;
    }
    protected bool AgentStopping()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    void StartDiscover()
    {
        discoverTimer = 0;
        isMoving = false;
        isDiscovering = true;
        anim.SetBool("Discover", true);
        anim.SetBool("Forward", false);
    }

    void _DiscoverComplete()
    {
        isDiscovering = false;
        GetComponent<LootInventory>().AddDiscoveredItem(prefab.Clone());
        if (nearPlayer) return;
        nextDestination();
    }

    void nextDestination()
    {
        if (nearPlayer) return;

        currentDestination = destinations[currentDestNumber].transform.position;
        currentDestNumber++;
        if (currentDestNumber >= destinations.Length)
        {
            currentDestNumber = 0;
        }
        agent.SetDestination(currentDestination);
        if (isDiscovering) isDiscovering = false;

        anim.SetBool("Forward", true);
        anim.SetBool("Discover", false);
        isDiscovering = false;
        isMoving = true;
    }

    void WaitForPlayer(bool act=true)
    {
        nearPlayer = act;
        if (isMoving)
        {
            anim.SetBool("Forward",!act);
        }
        else
        {
            if(!isDiscovering)
                nextDestination();
        }
    }
}
