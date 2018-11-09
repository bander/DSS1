using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MoveTypes {joystickForward,joysetickLocomotion,navigation };

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour {
    Animator anim;
    Joystick joystick;
    NavMeshAgent agent;

    Vector3 jDir;
    Vector3 keyDir;

    float rot=0;
    float rotationSpeed=160;
    MoveTypes moveType;

    public delegate void OnUpdate();
    public OnUpdate onUpdate;

    public delegate void OnArrived();
    public OnArrived onArrived;

    public GameObject navLocal;

    public GameObject pick;

    public GameObject SetTarget {
        set{ target = value; }
    }
    GameObject target;

    void Awake()
    {
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;

        onUpdate += MoveDefault;
    }
    

    public void ChangeMoveType(MoveTypes _mType)// Combat(int type=0)
    {
        switch (_mType)
        {
            case MoveTypes.joystickForward:
                onUpdate = MoveDefault;
                break;
            case MoveTypes.joysetickLocomotion:
                onUpdate = MoveInCombat;
                break;
            case MoveTypes.navigation:
                if (agent.isStopped) agent.isStopped = false;

                agent.destination = target.transform.position;
                agent.stoppingDistance = target.GetComponent<Interactable>().stopping;
                onUpdate = MoveNavigation;
                break;
        }

        /*if (type == 2) type = 1;

        if (type==0)
        {
            anim.SetBool("InCombat", false);
            onUpdate = MoveDefault;
        }
        else if(type==1)
        {
            anim.SetBool("InCombat", true);
            onUpdate = MoveInCombat;
        }
        else if (type==2)
        {
            anim.SetBool("InCombat", true);
            onUpdate = MoveNavigation;

            agent.destination = target.transform.position;
        }
        else if (type == 3)
        {
            anim.SetBool("InCombat", false);
            onUpdate = MoveCrouch;
        }
        else if (type == 4)
        {
            float dist = (transform.position - target.transform.position).magnitude;
            Debug.Log("dist "+dist);
            if (dist < 0.7f)
            {
                float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
                Vector3 cross = Vector3.Cross(transform.forward, target.transform.position - transform.position);
                if (cross.y < 0) angle = -angle;

                anim.SetFloat("PickupAngle", angle);
                anim.CrossFadeInFixedTime("TurnPickUp", 0.1f, 0, 0);
                anim.Update(0);
            }
            else
            {
                anim.SetBool("InCombat", false);
                agent.destination = target.transform.position;
                agent.stoppingDistance = 0.5f;
                onUpdate = MoveToPickup;
            }
        }
        //*/
        moveType = _mType;
    }

    bool demo = true;
    bool comb=false;
    public GameObject[] trashEnemy;
    int currentE = 0;
    public int mine = 1;
    int  trashCount;
    public GameObject trashPickup;
    void TrashForDemo()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (trashCount == 0)
            {
                trashCount++;
                anim.SetTrigger("pickPP");
                Destroy(trashPickup, 0.8f);
            }
            else
            {
                anim.SetInteger("Mine", mine);
                anim.SetTrigger("Miner");
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetFloat("WeaponNumber", -1);
            anim.SetInteger("WeaponType", -1);
//            activateCombat(0);
            comb = false;
        }
        if (!demo) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetFloat("WeaponNumber", 2);
            anim.SetInteger("WeaponType", 2);
//            activateCombat(1);
            comb = true;
        }

        if (trashEnemy[currentE].GetComponent<EnemyDemoScene>().dead)
            currentE++;

        if (currentE == trashEnemy.Length)
        {
            demo = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
//            anim.SetTrigger("Shoot");
//            trashShoot();
        }

        if (!comb) return;

        transform.LookAt(trashEnemy[currentE].transform);
    }

    public GameObject muzTr;
    public GameObject bull;
    public GameObject muz;
    public GameObject tra;
    public GameObject imp;
    public AudioClip aud;
    public float speed;
    void trashShoot()
    {
        if (muz != null)
        {
            Instantiate(muz, muzTr.transform.position, muzTr.transform.rotation);
        }
        if (aud != null)
        {
            GetComponent<AudioSource>().PlayOneShot(aud);
        }
        if(tra!=null || imp != null)
        {
            GameObject go = Instantiate(bull, muzTr.transform.position, muzTr.transform.rotation);
            go.GetComponent<BulletDemo>().SET(speed,trashEnemy[currentE],tra,imp);
        }
    }


    void Update() {
//        TrashForDemo();
        
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        
        jDir = cameraForward * joystick.Vertical + Camera.main.transform.right * joystick.Horizontal;
        keyDir = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");

        if (keyDir.magnitude > 0)
        {
            jDir = keyDir;
        }
        if(jDir.magnitude>0)
            if (moveType == MoveTypes.navigation)
                StopNavigateMotions();

        //Debug.Log("On "+onUpdate);
        if (onUpdate != null) onUpdate.Invoke();
    }

    void StopNavigateMotions()
    {
        if (!agent.isStopped) agent.isStopped = true;
        if (onUpdate != null) onUpdate = null;
        anim.SetInteger("Mine", 0);
        anim.SetTrigger("StopPickup");
        pick.SetActive(false);
        ChangeMoveType(MoveTypes.joystickForward);
    }

    
    void MoveToPickup()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        
        if (AgentStopping())
        {
            onUpdate = null;

            anim.SetFloat("InputMagnitude", 0);
            //anim.SetFloat("PickupAngle", 0);
            //anim.CrossFadeInFixedTime("TurnPickUp", 0.1f, 0, 0);
            anim.Update(0);
            return;
        }
        


        float angle = Vector3.Angle(transform.forward, agent.desiredVelocity);// jDir);
        Vector3 cross = Vector3.Cross(transform.forward, agent.desiredVelocity);// jDir);
        if (cross.y < 0) angle = -angle;

        SmoothRotation(angle);
        anim.SetFloat("InputMagnitude", Mathf.Min(agent.desiredVelocity.magnitude, 0.69f));// 0.69f);
        anim.SetFloat("InputAngle", rot);
        anim.SetFloat("RawInputAngle", angle);
    }
    

    void MoveDefault()
    {
        float ang = GetJoystickAngleRelativeToChar();
        SmoothRotation(ang);
        anim.SetFloat("InputMagnitude", jDir.magnitude);
        //anim.SetFloat("InputAngle", rot);
        anim.SetFloat("RawInputAngle", ang);
        if(jDir.magnitude>0.1)
            transform.rotation = Quaternion.LookRotation(jDir);// Euler(0, ang, 0);

    }
    void MoveInCombat()
    {
        Quaternion tr = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        tr.y = -tr.y;
        Vector3 strafeDirection = tr * jDir;
        anim.SetFloat("InputMagnitude", jDir.magnitude);
        anim.SetFloat("X", strafeDirection.x);
        anim.SetFloat("Z", strafeDirection.z);

        anim.SetBool("Shooting", RotateToEnemy());
    }
    void MoveCrouch()
    {
        Quaternion rotateTo = Quaternion.LookRotation(jDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * rotationSpeed);
        anim.SetFloat("InputMagnitude", jDir.magnitude);
    }
    void MoveNavigation()
    {
        Vector3 velocity = new Vector3(0, 0,0);
        if (!AgentDone())//(!AgentStopping())
        {
            velocity = agent.desiredVelocity;// Quaternion.Inverse(transform.rotation) * agent.desiredVelocity;
            velocity.y = 0;
            if(Quaternion.LookRotation(velocity)!=null) transform.rotation = Quaternion.LookRotation(velocity);
        }
        else
        {
            if (onUpdate != null) onUpdate = null;
            if (onArrived != null) onArrived.Invoke();
        }
        anim.SetFloat("InputMagnitude", velocity.magnitude);
    }
    void LookAtTarget()
    {
        if (target == null)
        {
            onUpdate = null;
            return;
        }
        Quaternion lookRot = Quaternion.LookRotation(target.transform.position-transform.position);
        if (Quaternion.Angle(transform.rotation,lookRot)>5)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
    }

    float GetJoystickAngleRelativeToChar()
    {
        float angle = Vector3.Angle(transform.forward, jDir);
        Vector3 cross = Vector3.Cross(transform.forward, jDir);
        if (cross.y < 0) angle = -angle;
        return angle;
    }
    void SmoothRotation(float angle)
    {
        if (angle > 1 && rot < angle) rot += (angle - rot) / 5;// rotSpeed * Time.deltaTime; 
        else if (angle < 1 && rot > angle) rot += (angle - rot) / 5;//rotSpeed * Time.deltaTime; 
        else
        {
            if (rot > 1) rot -= 1;
            else if (rot < 1) rot += 1;
            else rot = 0;
        }

    }
    
    
    void OnAnimatorMove()
    {
        agent.velocity = anim.deltaPosition / Time.deltaTime;
        transform.rotation = anim.rootRotation;
    }
    protected bool AgentDone()
    {
        return AgentStopping();// ||  !agent.pathPending;
    }
    protected bool AgentStopping()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    
    bool IsTargetInSight()
    {
        Ray ray = new Ray(transform.position, target.transform.position-transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            Debug.DrawLine(transform.position, hit.point,Color.red);
            if ((hit.point-target.transform.position).magnitude<1)
            {
                return true;
            }
        }
        return false;
    }

    bool RotateToEnemy()
    {
        if (target == null) return true;

        Quaternion direction = Quaternion.LookRotation(target.transform.position-transform.position);
        direction.x = 0;
        direction.z = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, direction,Time.deltaTime*450);
        Quaternion q = transform.rotation* Quaternion.Inverse(direction);
        
        if (q.y< 0.1) return true;
        return false;
    }


    ////////////////////  trash
    /// <summary>
    /// 
    public void StartPickUp()
    {
        anim.CrossFadeInFixedTime("TurnPickUp", 0.1f, 0, 0);
        anim.Update(0);
        onUpdate = LookAtTarget;
    }
    public void StartLoot()
    {
        anim.CrossFadeInFixedTime("TurnLoot", 0.1f, 0, 0);
        anim.Update(0);
        onUpdate = LookAtTarget;
    }
    public void StartMine()
    {
        pick.SetActive(true);

        anim.SetInteger("Mine",1);
        onUpdate = LookAtTarget;
    }
    public void EndMine()
    {
        pick.SetActive(false);
        anim.SetInteger("Mine", 0);
    }

    public void _LootFinish()
    {
        PlayerControl.instance._AnimPickupAtEnd();
    }
    public void _LootStart()
    {
        PlayerControl.instance._AnimPickupAtStart();
    }
}
