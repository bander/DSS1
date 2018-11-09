using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worm_control : MonoBehaviour {

    private enum wormStates
    {
        hidden,
        spawned,
        attack,
        dead
    }

    public Vector2 Radius = new Vector2(10f, 20f);
    public float RotationSpeed = 1f;
    public Vector2 AttackTime = new Vector2(10f, 20f);
    public Transform PlayerTransform;
    public GameObject SpawnFX;
    public GameObject Worm;

    private Animator animator;
    private wormStates state;
    private float attackTimer;

    void Start()
    {
        animator = Worm.GetComponent<Animator>();
        state = wormStates.hidden;
        Worm.SetActive(false);
        SpawnFX.SetActive(false);
    }

    void Update()
    {
        if (PlayerTransform == null)
            return;

        if (state == wormStates.dead)
            return;

        if (state == wormStates.hidden)
        {
            if (Vector3.Distance(transform.position, PlayerTransform.position) < Radius.x)
            {
                attackTimer = Random.Range(AttackTime.x, AttackTime.y);
                Worm.SetActive(true);
                SpawnFX.SetActive(true);
                animator.SetBool("hide", false);
                state = wormStates.spawned;
            }
        }
        else
        {
            if (state == wormStates.spawned)
            {
                Vector3 v = PlayerTransform.position - transform.position;
                v.y = 0;
                float dist = v.magnitude;

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(v, Vector3.up), Time.deltaTime * RotationSpeed);

                if (attackTimer < Time.time)
                {
                    animator.SetTrigger("attack");
                    attackTimer = Time.time + Random.Range(AttackTime.x, AttackTime.y);
                }

                if (Input.GetMouseButtonDown(0))
                    animator.SetTrigger("damage");

                if (Input.GetKeyDown(KeyCode.D))
                {
                    state = wormStates.dead;
                    animator.SetTrigger("death");
                }

                if (dist > Radius.y)
                {
                    animator.SetBool("hide", true);
                    state = wormStates.hidden;
                }
            }
        }

    }


    public void Hide()
    {
        Debug.Log("HIDE");
        state = wormStates.hidden;
        Worm.SetActive(false);
        SpawnFX.SetActive(false);
    }

    public void Spawned()
    {
        Debug.Log("SPAWNED");
        state = wormStates.spawned;
    }
}
