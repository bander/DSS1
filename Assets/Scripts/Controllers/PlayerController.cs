using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
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
    PlayerAnimator pAnim;
    InvManager manager;

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
    
    public delegate void OnMainUIUpdate();
    public OnMainUIUpdate onMainUIUpdate;

    public delegate void OnEnemyInRange();
    public OnEnemyInRange onEnemyInRange;

    public GameObject muzzlePrefab;
    AudioSource audioSource;

    public Transform muzzle;
    public AudioClip audioClip;

    public Transform[] muzzles;

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        joystick = FindObjectOfType<Joystick>();
        pAnim = GetComponent<PlayerAnimator>();

        manager = InvManager.instance;
        manager.OnInvChangedCallback += updateAttackType;

        agent.enabled = false;
    }
	
	void Update () {

        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x,0, Camera.main.transform.forward.z).normalized;
        moveDirection = cameraForward * joystick.Vertical + Camera.main.transform.right * joystick.Horizontal;
        moveDirection = new Vector3(0,0,0);// moveDirection.normalized * moveSpeed;

        if (controller.isGrounded)
        {
            vSpeed = 0; 
        }

        vSpeed -= gravity * Time.deltaTime;
        moveDirection.y = vSpeed;

        controller.Move(moveDirection * Time.deltaTime);

        if (false)//(joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            Quaternion modelRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, modelRotation, rotationSpeed * Time.deltaTime);

            if (inCombat == true)
            {
                inCombat = false;
                if (agent.enabled)
                {
                    if (!agent.isStopped)
                    {
                        agent.isStopped = true;
                    }
                    agent.enabled = false;
                }
            }
        }
        else
        {
            if (agent.enabled == true)
            {
                if (pathComplete())
                {
                    agent.isStopped = true;
                    agent.enabled = false;
                }
//              transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
                setFocus(enemy);
            }
            else
            {
                if (inCombat==true)
                {
                    if (CheckDistanceToEnemy())
                    {
                        if (lookAtEnemy())
                        {
                            StartAttack();
                        }
                    }
                    else
                    {
                        StartTrackCurrentEnemy(enemy);
                    }
                }
            }
        }
        if (onEnemyInRange != null) onEnemyInRange.Invoke();
    }

    public void StartTrackCurrentEnemy(Interactable newEnemy)
    {
        if (newEnemy != null)
        {
            inCombat = true;
            enemy = newEnemy;
            agent.enabled = true;
            agent.SetDestination(enemy.transform.position);
        }
    }
    public void StartAttack()
    {

    }
    public void Shoot()
    {
        //agent.stoppingDistance = 5;
        //muzzle = muzzles[3];
        //audioClip = audioClip;

        Instantiate(muzzlePrefab, muzzle.transform);
        AudioClip clip = audioClip;//audioSource.clip;
        audioSource.PlayOneShot(clip);

        if (enemy != null)
        {
            enemy.GetComponent<EnemyStats>().TakeDamage(GetComponent<PlayerStats>().damage.GetValue());
        }
    }

    int attackType;
    void updateAttackType()
    {
        Equipment weapon = InvManager.instance.GetCurrentWeapon();
        float attackDist = 2.5f;
        if (weapon != null)
        {
            attackDist = weapon.attackDistance;
            muzzlePrefab = weapon.muzzleEffect;
            audioClip = weapon.audioClip;
            int newWeapon = (int)weapon.attackType;
            muzzle = muzzles[newWeapon];

            if (attackType != newWeapon)
            {
                attackType = newWeapon;
                pAnim.animator.SetInteger("WeaponType", attackType);
                pAnim.animator.SetFloat("WeaponNumber", attackType);
                pAnim.animator.SetTrigger("SwitchWeapon");
                pAnim.showWeapon(attackType);
        }
        agent.stoppingDistance = attackDist;
    }
        else
        {
            if (attackType != -1)
            {
                attackType = -1;
                pAnim.animator.SetInteger("WeaponType", attackType);
            }
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
    bool lookAtEnemy()
    {
        bool ret = false;
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
        if (Quaternion.Angle(transform.rotation,rotation)<1) ret = true;
        return ret;
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
