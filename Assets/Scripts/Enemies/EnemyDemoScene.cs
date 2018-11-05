using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDemoScene : MonoBehaviour {

    NavMeshAgent agent;
    Animator anim;
    public GameObject player;
    public bool dead = false;

    public float rotationSpeed = 120;

    bool demo = false;

    void Start ()
    {
        if (!demo) return;


        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = PlayerManager.instance.player;

    }
    Vector3 velocity;
    void Update()
    {
        if (!demo) return;
        if (dead) return;
        agent.SetDestination(player.transform.position);
        velocity = agent.desiredVelocity;
        Move();
    }
    void Move()
    {
        if (!demo) return;

        float angle = Vector3.Angle(transform.forward, velocity);
        Vector3 cross = Vector3.Cross(transform.forward, velocity);
        if (cross.y < 0) angle = -angle;

        
        RotationNavigate();
        anim.SetBool("Walk Forward", true);
    }
    void RotationNavigate()
    {
        if (!demo) return;

        Vector3 direction = agent.desiredVelocity;
        direction.y = 0;

        Quaternion destRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, Time.deltaTime * rotationSpeed);
    }
    public int hits = 1;
    public void Death()
    {
        if (!demo) return;

        if (dead) return;
        hits--;
        if (hits > 0) return;
        agent.isStopped = true;
        anim.SetBool("Walk Forward", false);
        anim.SetTrigger("Die");
        transform.position = transform.position + Vector3.up * 0.5f;
        dead = true;
    }
}
