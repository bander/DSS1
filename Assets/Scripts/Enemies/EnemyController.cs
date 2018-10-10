using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    delegate void OnUpdate();
    OnUpdate onUpdate;

    EnemyStats stats;
    Animator anim;
    GameObject player;
    NavMeshAgent agent;

    Vector3 baseLocation;

    float attackDistance=3.5f;
    float validAngle = 7f;
    float rotationSpeed = 25f;


    void Start () {
        stats = GetComponent<EnemyStats>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = PlayerManager.instance.player;

        baseLocation = transform.position;

        stats.onHit += onTakeDamage;
	}
    void Update()
    {
        if (onUpdate != null) onUpdate.Invoke();
        //checkRotation();
    }
    void OnTakeDamage2()
    {
        //if (onUpdate == null) onUpdate += RotateToPlayer;
    }
    public void checkRotation()
    {
        if (!stats.dead)
        {
            Vector3 direction = (player.transform.position - transform.position);
            float dist = direction.magnitude;
            float angle = Vector3.Angle(transform.forward, direction);
            Vector3 cross = Vector3.Cross(transform.forward, direction);
            if (cross.y < 0) angle = -angle;
            
            if (Mathf.Abs(angle) < validAngle)
            {
                anim.SetFloat("Rotation",0);
                SmoothRotateToPlayer();

                if (dist > attackDistance)
                {
                    anim.SetBool("Walk Forward", true);
                    anim.SetBool("Attack", false);
                }
                else
                {
                    anim.SetBool("Walk Forward", false);
                    anim.SetBool("Attack", true);
                }

            }
            else
            {
                anim.SetFloat("Rotation", angle);
                anim.SetBool("Walk Forward", false);
                anim.SetBool("Attack", false);
            }
        }
        else
        {
            anim.SetFloat("Rotation", 0);
            anim.SetBool("Walk Forward", false);
            anim.SetBool("Attack", false);

        }
    }
    void SmoothRotateToPlayer()
    {
        Vector3 direction = player.transform.position- transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
    }


    void onTakeDamage()
    {
        if (true) return;

        if (!stats.dead)
        {
            agent.SetDestination(player.transform.position);
            anim.SetBool("Walk Forward", true);

            stats.onHit -= onTakeDamage;
            onUpdate += TrackAndAttackPlayer;
        }

    }
    void TrackAndAttackPlayer()
    {
        if (!stats.dead)
        {
            float dist = Vector3.Magnitude(transform.position - player.transform.position);

            if (dist <= agent.stoppingDistance)
            {
                anim.SetBool("Walk Forward", false);
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetBool("Walk Forward", true);
                anim.SetBool("Attack", false);
                agent.SetDestination(player.transform.position);
            }
        }
        else
        {
            anim.SetBool("Walk Forward", false);
            anim.SetBool("Attack", false);
        }
    }


    void Hit()
    {
        float dist = Vector3.Magnitude(transform.position - player.transform.position);
        if (dist <= agent.stoppingDistance+0.5f)
        {
            player.GetComponent<CharacterStats>().TakeDamage(10);
        }
    }
}
