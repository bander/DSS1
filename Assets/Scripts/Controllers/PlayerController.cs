using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    #region Singleton
    public static PlayerController instance;
    void Awake()
    {
        instance = this;
    }

    #endregion



    Interactable target;
    Camera cam;
    CharacterController controller;

    Vector3 moveDirection;
    float rotationSpeed = 12f;
    float moveSpeed = 4f;
    float walkSpeed = 2f;
    float runSpeed = 4f;
    float walkRotSpeed = 3f;
    float runRotSpeed = 12f;

    Joystick joystick;

    float gravity = 9.8f;
    private float vSpeed = 0f;

    Interactable enemy;
    bool inCombat= false;

    float attackDelay = 1f;
    float timeinterval;

    public delegate void OnMainUIUpdate();
    public OnMainUIUpdate onMainUIUpdate;

    public delegate void OnEnemyInRange();
    public OnEnemyInRange onEnemyInRange;

    void Start () {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        joystick = FindObjectOfType<Joystick>();
        
        agent.enabled = false;
    }
	
	void Update () {
        
        moveDirection = transform.forward * joystick.Vertical + transform.right * joystick.Horizontal;
        moveDirection = moveDirection.normalized * moveSpeed;

        if (controller.isGrounded)
        {
            vSpeed = 0; 
        }

        // apply gravity acceleration to vertical speed:
        vSpeed -= gravity * Time.deltaTime;
        moveDirection.y = vSpeed;

        controller.Move(moveDirection * Time.deltaTime);

        if (joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            Quaternion modelRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, modelRotation, rotationSpeed * Time.deltaTime);

            if (agent.enabled == true) agent.enabled = false;
        }
        else
        {
            if (agent.enabled == true)
            {
                if (pathComplete())
                {
                    agent.enabled = false;
                }
                transform.GetChild(0).rotation = Quaternion.LookRotation(agent.velocity.normalized);
                setFocus(enemy);
            }
            else
            {
                if (inCombat==true)
                {
                    if (CheckDistanceToEnemy())
                    {
                        lookAtEnemy();
                        AttackEnemy();
                    }
                    else
                    {
                        StartAttackCurrentEnemy(enemy);
                    }
                }
            }
        }

        if (onEnemyInRange != null) onEnemyInRange.Invoke();
        
    }

    public void StartAttackCurrentEnemy(Interactable newEnemy)
    {
        if (newEnemy != null)
        {
            inCombat = true;
            enemy = newEnemy;
            agent.enabled = true;
            agent.SetDestination(enemy.transform.position);
        }
    }
    protected bool pathComplete()
    {
        if (Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }
    bool CheckDistanceToEnemy()
    {
        if (enemy != null)
        {
            float dist = (enemy.transform.position - transform.position).magnitude;
            if (dist < agent.stoppingDistance)
            {
                return true;
            }
        }
        return false;
    }

    public void AttackEnemy()
    {
        timeinterval += Time.deltaTime;
        if (timeinterval >= attackDelay)
        {
            timeinterval = 0;
            enemy.GetComponent<EnemyStats>().TakeDamage(GetComponent<PlayerStats>().damage.GetValue());
        }
    }

    public void PickTarget(Interactable inter)
    {
            target = inter;
            lookAtTarget();
           // MenuScript.instance.removeImteractables(target);
            target.Interact();
    }

//prolonged interact 

    #region MoveByTap

    public LayerMask movementMask;
    NavMeshAgent agent;

    void CheckMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable inter = hit.collider.GetComponent<Interactable>();
                if (inter != null)
                {
                    setFocus(inter);
                }
                else
                {
                    if (hit.collider.tag == "Ground")
                    {
                        agent.SetDestination(hit.point);
                        removeFocus();
                        return;
                    }
                    else
                    {
                        removeFocus();
                        return;
                    }
                }
            }
        }
    }

    #endregion
    void setFocus(Interactable newFocus)
    {
        target = newFocus;
        //agent.stoppingDistance = target.radius*0.8f;
    }
    void removeFocus()
    {
        target = null;
        //agent.stoppingDistance = 0f;
    }

    void lookAtTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x,0f,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime*3f);
    }
    void lookAtEnemy()
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.GetChild(0).rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
    }

    public void SwitchRunWalk()
    {
        if (moveSpeed==runSpeed)
        {
            moveSpeed = walkSpeed;
            rotationSpeed = walkRotSpeed;
        }
        else
        {
            moveSpeed = runSpeed;
            rotationSpeed = runRotSpeed;
        }
            
        if (onMainUIUpdate != null) onMainUIUpdate.Invoke();

    }

    public bool isRunning()
    {
        bool ret = (moveSpeed == runSpeed) ? true: false;
        return ret;
    }
}
