using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour {
    public AnimationClip replacableAttackAnim;
    public AnimationClip[] defaultAttackAnimSet;
    protected AnimationClip[] currentAttackAnimSet;

    const float smoothTime = 2f;

    NavMeshAgent agent;
    CharacterController controller;
    PlayerController pController;
    protected Animator animator;
    protected CharacterCombat combat;

    public GameObject trashtarget = null;

    protected AnimatorOverrideController overrideController;

	protected virtual void Start () {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();

        pController = GetComponent<PlayerController>();
        if (pController.onMainUIUpdate != null) pController.onMainUIUpdate += updateWalkState;

        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        //if (trashtarget != null) agent.SetDestination(trashtarget.transform.position);

        currentAttackAnimSet = defaultAttackAnimSet;

        //combat.OnAttack += OnAttackl; 
	}
	

	protected virtual void Update () {
        float speedPercent = 0;//agent.velocity.magnitude / agent.speed;
        if (agent.enabled==true) {
            speedPercent = agent.velocity.magnitude;
        }
        else if (controller != null)
        {
            speedPercent = controller.velocity.magnitude;//agent.velocity.magnitude / agent.speed;
        }
        animator.SetFloat("speedPercent", speedPercent);//,smoothTime, Time.deltaTime);
        if (combat != null)
        {
            animator.SetBool("inCombat", combat.inCombat);
        }
        //*/
	}

    void updateWalkState()
    {
       animator.SetBool("isRunning",pController.isRunning());
    }

    protected virtual void OnAttack()
    {
        animator.SetTrigger("attack");
        int attackIndex = Random.Range(0,currentAttackAnimSet.Length);
        overrideController[replacableAttackAnim.name] = currentAttackAnimSet[attackIndex];
    }
}
