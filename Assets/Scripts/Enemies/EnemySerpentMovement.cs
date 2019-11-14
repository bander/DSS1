using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySerpentMovement : MonoBehaviour
{
    delegate void OnUpdate();
    OnUpdate onUpdate;

    Animator anim;
    NavMeshAgent agent;
    EnemyStats stats;

    GameObject player;
    PlayerStats pStats;


    float visibleAngle = 60;
    float rotationSpeed = 120;

    Vector3 velocity;
    public float validMoveAngle = 30;

    public float attackDist = 0.7f;
    public float damageMin = 3;
    public float damageMax = 3;

    public bool isAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<EnemyStats>();

        player = PlayerManager.instance.player;
        pStats = player.GetComponent<PlayerStats>();

        agent.enabled = true;
        agent.updateRotation = false;

        agent.stoppingDistance = attackDist;

        stats.onHit += StartFollow;

        //StartFollow();
    }

    public void StartFollow()
    {
        if (stats.dead) return;

        agent.enabled = true;
         onUpdate = MoveNavDirection;
    }

    void Arrive()
    {
        onUpdate = RotationToPlayer;
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Attack1", true);
        agent.enabled = false;
    }
    void Update()
    {
        if (onUpdate != null) onUpdate.Invoke();
    }

    void MoveNavDirection()
    {
        if (stats.dead)
        {
            onUpdate = null;
            return;
        }

        agent.SetDestination(player.transform.position);
        if (!AgentStopping())
        {
            //velocity = Quaternion.Inverse(transform.rotation) * agent.desiredVelocity;
            velocity = agent.desiredVelocity;
        }
        else
        {
            Arrive();
            return;
        }

        Move();
    }

    void Move()
    {
        if (stats.dead) return;

        float angle = Vector3.Angle(transform.forward, velocity);
        Vector3 cross = Vector3.Cross(transform.forward, velocity);
        if (cross.y < 0) angle = -angle;
        
        //if (Mathf.Abs(angle) < validMoveAngle)
        //{
            anim.SetFloat("Rotation", 0);
            RotationNavigate();

            anim.SetBool("Walk Forward", true);
            anim.SetBool("Attack1", false);
            /*
        }
        else
        {
            anim.SetFloat("Rotation", angle);
            anim.SetBool("Walk Forward", false);
            anim.SetBool("Attack", false);

            Debug.Log(" ROTATE "+angle);
        }
        //*/
    }

    bool AgentStopping()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist <= agent.stoppingDistance;
        //return agent.remainingDistance <= agent.stoppingDistance;
    }
    void RotationNavigate()
    {
        if (stats.dead) return;

        Vector3 direction = (AgentStopping()) ? player.transform.position - transform.position : agent.desiredVelocity;
        direction.y = 0;

        Quaternion destRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, Time.deltaTime * rotationSpeed);
    }

    void RotationToPlayer()
    {
        if (stats.dead) return;

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        Quaternion destRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, Time.deltaTime * rotationSpeed);

        if (direction.magnitude > attackDist)
        {
            StartFollow();
        }
    }

    bool IsPlayerInSight()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, direction) < visibleAngle) return true;
        return false;
    }
    bool IsPlayerVisible()
    {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if ((hit.point - player.transform.position).magnitude < 1)
            {
                return true;
            }
        }
        return false;
    }

    void _AnimAttack()
    {
        if (stats.dead) return;

        float dist = (player.transform.position - transform.position).magnitude;

        if (dist > attackDist + 0.1f)
        {
            StartFollow();
            return;
        }


        pStats.TakeDamage((int)Random.Range(damageMin,damageMax));
    }
}
