using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour {

    public float attackSpeed=1f;
    float attackCountdown=0f;

    public float combatCooldown = 5f;
    float lastAttackTime;

    public float attackDelay = .6f;
    public bool inCombat { get; private set; }
    public event System.Action OnAttack;

    CharacterStats stats;

    void Start () {
        stats = GetComponent<CharacterStats>();
	}

    void Update()
    {
        attackCountdown -= Time.deltaTime;

        if(Time.time - lastAttackTime > combatCooldown)
        {
            inCombat = false;
        }
    }
	
	public void Attack(CharacterStats targetStats)
    {
        if (attackCountdown < 0f)
        {
            //targetStats.TakeDamage(stats.damage.GetValue());
            StartCoroutine(DoDamage(targetStats,attackDelay));

            if (OnAttack != null) OnAttack();

            attackCountdown = 1f / attackSpeed;
            inCombat = true;
            lastAttackTime = Time.time;
        }
    }

    IEnumerator DoDamage(CharacterStats stats2,float delay)
    {
        yield return new WaitForSeconds(delay);
        stats2.TakeDamage(stats.damage.GetValue());

        if (stats2.currentHealth <= 0)
        {
            inCombat = false;
        }
             
    }
}
