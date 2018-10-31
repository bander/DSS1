using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour {

    /*
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        joysDirection = cameraForward * vJoystick.Vertical + Camera.main.transform.right * vJoystick.Horizontal;
        
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float magn = joysDirection.magnitude;
        float ang=0;
        if (magn>0) {
            Vector3 end = joysDirection;
            Debug.DrawLine(new Vector3(0,0,0),end*4);

            ang = Vector3.Angle(transform.forward, end);
            Vector3 cross = Vector3.Cross(transform.forward, end);
            if (cross.y < 0) ang = -ang;

            if (prevClip != animator.GetCurrentAnimatorClipInfo(0)[0].clip.name)
            {
                clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                rotCurrent = 0;
                animator.SetFloat("WalkStartAngle", ang);
            }

            
            if (ang > 1 && rotCurrent < ang) rotCurrent += (ang - rotCurrent)/5;// rotSpeed * Time.deltaTime; 
            else if (ang < 1 && rotCurrent > ang) rotCurrent += (ang - rotCurrent)/5;//rotSpeed * Time.deltaTime; 
            else
            {
                if (rotCurrent > 1) rotCurrent -= 1;
                else if (rotCurrent < 1) rotCurrent += 1;
                else rotCurrent = 0;
            }

            if (prevClip !=clip)
            {
                prevClip = clip;
            }
        }

        animator.SetFloat("InputMagnitude", magn);
        animator.SetFloat("InputAngle", rotCurrent);
        animator.SetFloat("RawInputAngle", ang);
        
        if (Input.GetKeyDown(KeyCode.Space)) inCombat = true;
        if (Input.GetKeyUp(KeyCode.Space)) inCombat = false;
       animator.SetBool("InCombat", inCombat);

        if (inCombat)
        {
            Quaternion tr = transform.rotation;
            tr.y = -tr.y;
            Vector3 strafeDirection = tr * joysDirection;
            animator.SetFloat("X", strafeDirection.x);
            animator.SetFloat("Z", strafeDirection.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("Shoot");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("Shooting",true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("Shooting", false);
        }

//*/

        
    Animator anim;
    Joystick joystick;
    NavMeshAgent agent;

    Vector3 jDir;
    Vector3 keyDir;

    float rot=0;
    float rotationSpeed=160;
    string currentClip;
    int moveType;

    public delegate void OnUpdate();
    public OnUpdate onUpdate;

    public delegate void OnArrived();
    public OnArrived onArrived;

    GameObject target;

    public GameObject navLocal;

    public GameObject SetTarget {
        set{ target = value; } }

    void Awake()
    {
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;

        onUpdate += MoveDefault;
    }

    public void SetCrouch(bool newCrouch)
    {
        activateCombat(3);
        anim.SetBool("Crouch", newCrouch);
    }
    public void activateCombat(int type=0)
    {
        if (type == 2) type = 1;

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
        moveType = type;
    }

    bool demo = true;
    bool comb=false;
    public GameObject[] trashEnemy;
    int currentE = 0;
    void TrashForDemo()
    {
        if (!demo) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetFloat("WeaponNumber", 2);
            anim.SetInteger("WeaponType", 2);
            activateCombat(1);
            comb = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetFloat("WeaponNumber", -1);
            anim.SetInteger("WeaponType", -1);
            activateCombat(0);
            comb = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Shoot");
            //trashEnemy[currentE].GetComponent<EnemyDemoScene>().Death();
            trashShoot();

            if (currentE == trashEnemy.Length)
            {
                demo = false;
                return;
            }
            //currentE++;
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
        TrashForDemo();

        if (Input.GetKeyDown(KeyCode.P))
        {
            // trash = !trash;
            // navLocal.SetActive(trash);
        }

        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        
        jDir = cameraForward * joystick.Vertical + Camera.main.transform.right * joystick.Horizontal;
        keyDir = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
        
       if(keyDir.magnitude > 0)  jDir = keyDir;

       if (moveType == 2 && jDir.magnitude > 0) activateCombat(1);

       if (onUpdate != null) onUpdate.Invoke();
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
        if (!AgentStopping())
        {
            velocity = Quaternion.Inverse(transform.rotation) * agent.desiredVelocity;
        }
        else
        {
            if (onArrived != null) onArrived.Invoke();
            if (onUpdate != null) onUpdate = null;
        }
        anim.SetFloat("InputMagnitude", velocity.magnitude);
        anim.SetFloat("X", velocity.x);
        anim.SetFloat("Z", velocity.z);

        RotationNavigate();
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
    void CheckAnimStates(float ang)
    {
        AnimatorStateInfo nextState = anim.GetNextAnimatorStateInfo(0);
        if (nextState.IsName("RunFwdStart"))
        {

        }
        string nextClip = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (nextClip!=currentClip)
        {

        }
    }

    
    void OnAnimatorMove()
    {
        agent.velocity = anim.deltaPosition / Time.deltaTime;
        transform.rotation = anim.rootRotation;
    }
    protected bool AgentDone()
    {
        return !agent.pathPending || AgentStopping();
    }
    protected bool AgentStopping()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    void RotationNavigate()
    {
        Vector3 direction =  (IsTargetInSight())? target.transform.position - transform.position: agent.desiredVelocity;
        direction = IsTargetInSight()? target.transform.position - transform.position: agent.desiredVelocity;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
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
}
