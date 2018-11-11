using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    delegate void OnUpdate();
    OnUpdate onUpdate;

    NavMeshAgent agent;
    Animator anim;
    EnemyStats stats;

    GameObject player;
    PlayerStats pStats;

    float visibleAngle = 60;
    float rotationSpeed = 120;

    Vector3 velocity;
    public float validMoveAngle = 60;

    public float attackDist=0.7f;
    public float damage=3;

    public bool followPlayerAtStart = false;

    public SpiderLeg[] legs;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();

        player = PlayerManager.instance.player;
        pStats = player.GetComponent<PlayerStats>();

        agent.stoppingDistance = attackDist;
        agent.enabled = true;
        agent.updateRotation = false;
        //agent.updatePosition = false;

        if (followPlayerAtStart)
            StartFollow();
        else
            stats.onHit += StartFollow;

        if (legs != null)
        {
            foreach (SpiderLeg leg in legs)
            {
                leg.onIntersect += LegHit;
            }
        }

	}
	
    public void StartFollow()
    {
        if (stats.dead) return;

        agent.enabled = true;

        onUpdate = null;
        onUpdate += MoveNavDirection;

    }
    void Arrive()
    {
        onUpdate = null;
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Attack", true);
        agent.enabled = false;
    }

	void Update ()
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
        float angle = Vector3.Angle(transform.forward, velocity);
        Vector3 cross = Vector3.Cross(transform.forward, velocity);
        if (cross.y < 0) angle = -angle;
        

        if (Mathf.Abs(angle) < validMoveAngle)
        {
            anim.SetFloat("Rotation", 0);
            RotationNavigate();
            
                anim.SetBool("Walk Forward", true);
                anim.SetBool("Attack", false);

        }
        else
        {
            anim.SetFloat("Rotation", angle);
            anim.SetBool("Walk Forward", false);
            anim.SetBool("Attack", false);
        }
    }

    bool AgentStopping()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    void RotationNavigate()
    {
        Vector3 direction = (AgentStopping()) ? player.transform.position - transform.position : agent.desiredVelocity;
        direction.y = 0;

        Quaternion destRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, Time.deltaTime * rotationSpeed);
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
        float dist = (player.transform.position - transform.position).magnitude;

        if (dist > attackDist+0.4f)
        {
            StartFollow();
            return;
        }

        pStats.TakeDamage(damage);
    }

    void LegHit()
    {
        float dist = Vector3.Magnitude(transform.position - player.transform.position);
        if (dist <= agent.stoppingDistance + 0.4f)
            pStats.TakeDamage(damage);
        else
            StartFollow();
    }
}
