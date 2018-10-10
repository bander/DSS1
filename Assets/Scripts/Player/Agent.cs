using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour {
    
	protected UnityEngine.AI.NavMeshAgent		agent;
	protected Animator			animator;

	protected Locomotion locomotion;

    public GameObject cube;

    Camera cam;


    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.stoppingDistance = 5f;
        agent.enabled = true;

        animator = GetComponent<Animator>();
        locomotion = new Locomotion(animator);

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        SetDestination();
	}
    public GameObject target;
	protected void SetDestination()
	{
        animator.SetBool("InCombat", true);
        animator.SetFloat("WeaponNumber", 3);
        animator.SetInteger("WeaponType", 3);

		Quaternion q = new Quaternion();
		q.SetLookRotation(target.transform.position, Vector3.forward);
        if (cube != null)
			Instantiate(cube, target.transform.position+Vector3.up*2, q);

		agent.destination = target.transform.position;
        //agent.SetDestination(target.transform.position);
    }

	//protected void SetupAgentLocomotion()
    void Update()
	{
		if (AgentDone())
		{
			locomotion.Do(0, 0);
		}
		else
		{
			float speed = agent.desiredVelocity.magnitude;

			Vector3 velocity = Quaternion.Inverse(transform.rotation) * agent.desiredVelocity;

			float angle = Mathf.Atan2(velocity.x, velocity.z) * 180.0f / 3.14159f;

			locomotion.Do(speed, angle);
		}
	}

    void OnAnimatorMove()
    {
        agent.velocity = animator.deltaPosition / Time.deltaTime;
		transform.rotation = animator.rootRotation;
    }

	protected bool AgentDone()
	{
		return !agent.pathPending && AgentStopping();
	}

	protected bool AgentStopping()
	{
		return agent.remainingDistance <= agent.stoppingDistance;
	}
}
