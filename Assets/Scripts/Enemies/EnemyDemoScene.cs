using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDemoScene : MonoBehaviour {

    NavMeshAgent agent;
    Animator anim;
    public GameObject player;
    bool dead = false;

    public float rotationSpeed = 120;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = PlayerManager.instance.player;

    }
    Vector3 velocity;
    void Update()
    {
        if (dead) return;
        agent.SetDestination(player.transform.position);
        velocity = agent.desiredVelocity;
        Move();
    }
    void Move()
    {
        float angle = Vector3.Angle(transform.forward, velocity);
        Vector3 cross = Vector3.Cross(transform.forward, velocity);
        if (cross.y < 0) angle = -angle;

        
        RotationNavigate();
        anim.SetBool("Walk Forward", true);
    }
    void RotationNavigate()
    {
        Vector3 direction = agent.desiredVelocity;
        direction.y = 0;

        Quaternion destRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, Time.deltaTime * rotationSpeed);
    }

    public void Death()
    {
        agent.isStopped = true;
        dead = true;
        anim.SetBool("Walk Forward", false);
        anim.SetTrigger("Die");

    }
}
