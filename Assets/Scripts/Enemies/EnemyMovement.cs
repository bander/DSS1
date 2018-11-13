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

    public bool startRoll = false;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();

        player = PlayerManager.instance.player;
        pStats = player.GetComponent<PlayerStats>();

        agent.enabled = true;
        agent.updateRotation = false;

        if(startRoll)
            agent.stoppingDistance = 1.5f;
        else
            agent.stoppingDistance = attackDist;


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

        if (startRoll)
        {
            startRoll = false;
            anim.SetBool("Roll", true);
            onUpdate = MoveRoll;
        }
        else
            onUpdate = MoveNavDirection;

    }
    void Arrive()
    {
        anim.SetBool("Roll", false);
        onUpdate = RotationToPlayer;
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Attack", true);
        agent.enabled = false;
    }

    public int trashGroup = 0;
	void Update ()
    {
        if (onUpdate != null) onUpdate.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (trashGroup == 1)
            {
                StartFollow();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (trashGroup == 2)
            {
                StartFollow();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (trashGroup == 3)
            {
                StartFollow();
            }
        }
    }
    

    void MoveNavDirection()
    {
        if (stats.dead)
        {
            onUpdate = null;
            return;
        }

        agent.SetDestination(player.transform.position);
        //Debug.Log("ag rem "+agent.remainingDistance);
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
    void MoveRoll()
    {
        if (stats.dead)
        {
            onUpdate = null;
            return;
        }

        agent.SetDestination(player.transform.position);

        if (!AgentStopping())
        {
            velocity = agent.desiredVelocity;
        }
        else
        {
            agent.stoppingDistance = attackDist;
            anim.SetBool("Roll", false);
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
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist <= agent.stoppingDistance;
        //return agent.remainingDistance <= agent.stoppingDistance;
    }
    void RotationNavigate()
    {
        Vector3 direction = (AgentStopping()) ? player.transform.position - transform.position : agent.desiredVelocity;
        direction.y = 0;

        Quaternion destRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, Time.deltaTime * rotationSpeed);
    }
    void RotationToPlayer()
    {
        Vector3 direction =  player.transform.position - transform.position;
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
        float dist = (player.transform.position - transform.position).magnitude;

        if (dist > attackDist+0.1f)
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
