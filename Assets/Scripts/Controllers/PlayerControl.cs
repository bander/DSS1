using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    #region Singleton
    public static PlayerControl instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    delegate void OnAttack();
    OnAttack onAttack;
    delegate void OnAttackBegin();
    OnAttackBegin onAttackBegin;
    delegate void OnAttackEnd();
    OnAttackEnd onAttackEnd;

    delegate void OnUpdate();
    OnUpdate onUpdate;

    PlayerStats stats;
    Movement movem;
    Animator anim;
    ShootScript shootScript;

    GameObject enemy;
    bool enemyLow;

    public GameObject[] weapons;

    public Item ite;

    void trashSAveTesT()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            /*
            SaveGameInventory.Instance.itemsInv = new List<Item>();
            InvManager.instance.invents[0].Add(ite);
            Debug.Log("ADD "+ InvManager.instance.invents[0].items.Count);
            /*
            SaveGameInventory.Instance.itemsInv.Add(ite);
            Debug.Log("Save -- "+SaveGameInventory.Instance.itemsInv.Count);
            int i = 0;
            foreach (Item itt in SaveGameInventory.Instance.itemsInv)
            {
                if (itt != null)
                {

                }
                i++;
            }
            SaveGameInventory.Save();
            //*/
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            /*
            SaveGameInventory.Load();

            int i = 0;
            foreach(Item itt in SaveGameInventory.Instance.itemsInv)
            {
                if (itt != null)
                {

                    Debug.Log(i + "  LOAD  " + itt.name);
                }
                i++;
            }
            //*/
        }
    }

    void Update()
    {
        trashSAveTesT();

        if (onUpdate != null) onUpdate.Invoke();

        //////////////////
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            AttackStart();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AttackStop();
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            GoToPickup();
        }
    }

    void CheckCurerntTarget()
    {
        Interactable enemyInter = PlayerInteractions.instance.FindNearestEnemy();
        if (enemyInter == null) return;
        GameObject newEnemy = enemyInter.gameObject;
        
        if (newEnemy != enemy)
        {
            enemy = newEnemy;
            enemyLow = enemyInter.low;

            if (enemy!=null)
            {
                movem.SetTarget = enemy;
            }
        }
    }

	void Start () {
        shootScript = GetComponent<ShootScript>();
        anim = GetComponent<Animator>();
        movem = GetComponent<Movement>();
        //stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats = GetComponent<PlayerStats>();

        stats.onStatsUpdate += UpdateAttackState;
        UpdateAttackState();

        
    }
	
	void UpdateAttackState () {
        int num = stats.WepaonNum;

        anim.SetInteger("WeaponType", num);
        anim.SetFloat("WeaponNumber", num);
        anim.SetTrigger("SwitchWeapon");

        showWeapon(num);
        
        onAttack = null;
        onAttackBegin = null;
        onAttackEnd = null;

        switch (num)
        {
            case 0:
                onAttack = KickOnce;
                break;
            case 1:
                onAttack = MeleeOnce;
                break;
            case 2:
                onAttack = ShootOnce;
                break;
            case 3:
                onAttackBegin = StartShooting;
                onAttackEnd = StopShooting;
                break;
        }
	}

    public void Attack()
    {
        if (onAttack != null) onAttack.Invoke();
    }
    public void AttackStart()
    {
        if (onAttackBegin != null) onAttackBegin.Invoke();
    }
    public void AttackStop()
    {
        if (onAttackEnd != null) onAttackEnd.Invoke();
    }

    void KickOnce()
    {
        int random = Random.Range(0, 2);
        anim.SetInteger("RandomAttack", random);

        CheckCurerntTarget();
        movem.ChangeMoveType(MoveTypes.joysetickLocomotion);//activateCombat(1);
        anim.SetTrigger("Shoot");
        timer = 0;

        if (!pistolResetter)
        {
            onUpdate += CheckCurerntTarget;
            onUpdate += ResetCombatModeafterPistolShoot;
            pistolResetter = true;
        }
    }
    void MeleeOnce()
    {
        if (enemyLow)
        {
            int random = Random.Range(1, 4);
            anim.SetInteger("RandomAttack",random);
        }
        else
            anim.SetInteger("RandomAttack",0);


        CheckCurerntTarget();
        movem.ChangeMoveType(MoveTypes.navigation);//.activateCombat(2);
        anim.SetTrigger("Shoot");
        timer = 0;

        if (!pistolResetter)
        {
            onUpdate += CheckCurerntTarget;
            onUpdate += ResetCombatModeafterPistolShoot;
            pistolResetter = true;
        }
    }
    void ShootOnce()
    {
        Debug.Log("shoot ");
        movem.ChangeMoveType(MoveTypes.joysetickLocomotion);//.activateCombat(1);
        anim.SetTrigger("Shoot");
        anim.SetBool("InCombat",true);
        timer = 0;
        if (!pistolResetter)
        {
            onUpdate += CheckCurerntTarget;
            onUpdate += ResetCombatModeafterPistolShoot;
            pistolResetter = true;
        }

            //anim.SetBool("InCombat", true);
    }

    float timer;
    bool pistolResetter = false;
    void ResetCombatModeafterPistolShoot()
    {
        timer += Time.deltaTime;
        if (timer > 6)
        {
            timer = 0;
            anim.SetBool("InCombat", false);
            onUpdate -= ResetCombatModeafterPistolShoot;
            onUpdate -= CheckCurerntTarget;
            movem.ChangeMoveType(MoveTypes.joystickForward);//.activateCombat(0);
            pistolResetter = false;
        }
    }
    void StartShooting()
    {
        onUpdate += CheckCurerntTarget;
        movem.ChangeMoveType(MoveTypes.joysetickLocomotion);//.activateCombat(1);
    }
    void StopShooting()
    {
        onUpdate -= CheckCurerntTarget;
        movem.ChangeMoveType(MoveTypes.joystickForward);//.activateCombat(0);
    }

    public void _AnimShoot()
    {
        shootScript.Fire(enemy);
    }

    public void _AnimStartMelee()
    {
        anim.SetBool("ReadyToAttack", false);
        //weapons[0].GetComponent<SwordHit>().StartTargetChecking(enemy);
        //weapons[0].GetComponent<SwordHit>().onIntersect += MeleeHit;
    }
    public void _AnimStopMelee()
    {
        anim.SetBool("ReadyToAttack", true);
        //weapons[0].GetComponent<SwordHit>().StopTargetChecking();
        //weapons[0].GetComponent<SwordHit>().onIntersect -= MeleeHit;
    }
    public void _AnimAttackStart()
    {
        anim.SetBool("ReadyToAttack", false);
    }
    public void _AnimAttackFinish()
    {
        anim.SetBool("ReadyToAttack", true);
    }

    void MeleeHit()
    {
        shootScript.Fire(enemy);
    }
    void _AnimHit()
    {
        shootScript.Fire(enemy);
    }


    //public List<Interactable> trash;
    Interactable currentPickup;
    public void GoToPickup()//Interactable inter)
    {
        currentPickup=PlayerInteractions.instance.FindNearestItemToPlayer();
        if (currentPickup == null) return;

        movem.SetTarget=currentPickup.gameObject;
        movem.onArrived = ArriveToPickTarget;
        movem.ChangeMoveType(MoveTypes.navigation);//.activateCombat(4);
    }
    public void ArriveToPickTarget()
    {
        movem.onArrived = null;
        if(currentPickup is LootInventory)
        {
            movem.StartLoot();
        }
        if (currentPickup is Pickup)
        {
            movem.StartPickUp();
        }
        if (currentPickup is PickupMine)
        {
            if (InvManager.instance.HasPickInBackpack())
                movem.StartMine();
            else
                Debug.Log("Сообщение о кирке");
        }
        //currentPickup.Interact();
    }
    public void _AnimPickupAtEnd()
    {
        if (currentPickup != null)
        {
            currentPickup.closed = false;
            currentPickup.Interact();
        }
    }
    public void _AnimPickupAtStart()
    {
        if (currentPickup != null && !currentPickup.closed)
        {
            currentPickup.Interact();
        }
    }
    public void _AnimMineEnd()
    {
        if (currentPickup != null && !currentPickup.closed)
        {
            currentPickup.Interact();
            if(currentPickup is PickupMine)
            {
                PickupMine pm = (PickupMine)currentPickup;
                if(pm.iterations<0)
                    movem.EndMine();
            }
        }
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
}
