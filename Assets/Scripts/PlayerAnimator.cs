using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour {
    /*
    const float smoothTime = 2f;

    CharacterController controller;
    PlayerController pController;
    public Animator animator;
    protected CharacterCombat combat;
    InvManager manager;

    public GameObject trashtarget = null;

    protected AnimatorOverrideController overrideController;

    public Transform muzzle;
    public Transform muzzleMachine;
    public GameObject muzzlePrefab;

    public GameObject[] weapons;

    Joystick vJoystick;
    AudioSource audioSource;

    public AudioClip swordAudio;
    public AudioClip pistolAudio;
    public AudioClip machineAudio;

    Vector3 joysDirection;

    int attackType = -1;
    int storedAttackType = -1;

    protected virtual void Start ()
    {
        vJoystick = FindObjectOfType<Joystick>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //combat = GetComponent<CharacterCombat>();

        pController = GetComponent<PlayerController>();

        animator = GetComponent<Animator>();

        if (pController.onMainUIUpdate != null)
            pController.onMainUIUpdate += updateWalkState;
	}

    public GameObject trr;
    string prevClip;
    string clip;
    float rotCurrent;
    float rotSpeed = 260f;
    float rotSpeedBack = 260f;
    float rotMaxAngle = 140;
    float currentWeapon;

    bool inCombat;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) trashSetWEapon(-1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) trashSetWEapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha3)) trashSetWEapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha4)) trashSetWEapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha5)) trashSetWEapon(3);


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

        //animator.SetFloat("InputMagnitude", magn);
        //animator.SetFloat("InputAngle", rotCurrent);
        //animator.SetFloat("RawInputAngle", ang);
        
        if (Input.GetKeyDown(KeyCode.Space)) inCombat = true;
        if (Input.GetKeyUp(KeyCode.Space)) inCombat = false;
 //      animator.SetBool("InCombat", inCombat);

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
    }

    void trashSetWEapon(int num)
    {
        trashNUM = num;
        updateAttackType();
        animator.SetInteger("WeaponNumber", num);
    }
    int trashNUM;
    void updateAttackType()
    {
       
    }

	protected virtual void UpdateXxxx () {
        float speedPercent = 0;//agent.velocity.magnitude / agent.speed;
        
        if (controller != null)
        {
            speedPercent = controller.velocity.magnitude;//agent.velocity.magnitude / agent.speed;
            if (speedPercent > 0.1f && machineShooting)
            {
                machineShooting = false;
            }
        }
        animator.SetFloat("speedPercent", speedPercent);//,smoothTime, Time.deltaTime);
        if (combat != null)
        {
            animator.SetBool("inCombat", combat.inCombat);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            attackType = -1;
            animator.SetInteger("WeaponType", attackType);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {;
            attackType = 0;
            animator.SetInteger("WeaponType", attackType);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            attackType = 1;
            animator.SetInteger("WeaponType", attackType);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            attackType = 2;
            animator.SetInteger("WeaponType", attackType);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            attackType = 3;
            animator.SetInteger("WeaponType", attackType);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            int ff = Random.Range(0,3);
            animator.SetInteger("HitType",0 );
            animator.SetTrigger("HardHit");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            int ff = Random.Range(0, 2);
            animator.SetInteger("HitType", ff);
            animator.SetTrigger("LightHit");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (speedPercent < 0.1f)
            {
                int ff = Random.Range(0, 8);
                animator.SetInteger("HitType", ff);
                animator.SetTrigger("Kick");
                Instantiate(muzzlePrefab, muzzle.transform);
                AudioClip clip = pistolAudio;//audioSource.clip;
                audioSource.PlayOneShot(clip);

            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (speedPercent < 0.1f)
            {
                animator.SetBool("Shooting", true);
                machineTimer = 0;
                machineShooting = true;
            }
              
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            machineShooting = false;
            animator.SetBool("Shooting", false);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Kick");
        }
        if (machineShooting)
        {
            machineTimer += Time.deltaTime;
            if (machineTimer > machineDelay)
            {
                machineTimer = 0;
                Instantiate(muzzlePrefab, muzzleMachine.transform);
                AudioClip clip = machineAudio;//audioSource.clip;
                audioSource.PlayOneShot(clip);
            }
        }
    }
    bool machineShooting=false;
    float machineTimer;
    float machineDelay=0.15f;

    void updateWalkState()
    {
       animator.SetBool("isRunning",pController.isRunning());
    }
    
    public void showWeapon(int num)
    {
        foreach (GameObject w in weapons)
        {
            w.SetActive(false);
        }
        num -= 1;
        if (num >= 0)
        {
            weapons[num].SetActive(true);
        }
    }
    //*/
}
