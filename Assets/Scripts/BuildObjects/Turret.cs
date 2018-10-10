using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    delegate void Alarm();
    Alarm alarm;

    public float radius = 7;
    GameObject[] enemies;
    List<GameObject> enemiesInRange = new List<GameObject>();
    AudioSource audio;
    GameObject currentEnemy;
    EnemyStats stats;

    public GameObject lamp;
    public GameObject muzzle;
    public float rotationSpeed=120;

    public Transform muzzleTransform;
    public AudioClip clip;
    public GameObject effect;   

    float timer;
    float fireDelay = 0.05f;
    float damage = 1;

	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        audio = GetComponent<AudioSource>();
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    void Update () {
        UpdateInRangeList();
        SetAlarm();
        if (alarm != null) alarm.Invoke();
	}
    void UpdateInRangeList()
    {
        foreach (GameObject enemy in enemies)
        {
            if ((enemy.transform.position - transform.position).magnitude < radius) {

                if (enemy.GetComponent<CharacterStats>().dead)
                {
                    if (enemiesInRange.Contains(enemy)) enemiesInRange.Remove(enemy);
                }
                else{
                    if (!enemiesInRange.Contains(enemy)) enemiesInRange.Add(enemy);
                }

            }
            else
                if (enemiesInRange.Contains(enemy)) enemiesInRange.Remove(enemy);
        }
    }
    void SetAlarm()
    {
        if (enemiesInRange.Count > 0) alarm = Attack; else alarm = null;
    }
    void Attack()
    {
        FindNearestEnemy();

        if (RotateToEnemy()) FireTimer(); else StopFire();
    }

    void FireTimer()
    {
        timer += Time.deltaTime;
        if (timer > fireDelay)
        {
            timer = 0;
            Fire();
        }
    }
    void StopFire()
    {
        timer = 0;
    }

    void Fire()
    {
        if (clip != null) audio.PlayOneShot(clip);
        if (muzzleTransform != null) Instantiate(effect, muzzleTransform);
        stats.TakeDamage(damage);
    }

    void FindNearestEnemy()
    {
        float dist = (enemiesInRange[0].transform.position - transform.position).magnitude;
        currentEnemy = enemiesInRange[0];
        stats = currentEnemy.GetComponent<EnemyStats>();
        foreach (GameObject enemy in enemiesInRange)
        {
                float dist2 = (enemy.transform.position - transform.position).magnitude;
            if (dist2 < dist)
            {
                dist = dist2;
                currentEnemy = enemy;
                stats = currentEnemy.GetComponent<EnemyStats>();
            }
        }
    }
    bool RotateToEnemy()
    {
        if (currentEnemy != null)
        {
            Vector3 direction = currentEnemy.transform.position - transform.position;
            muzzle.transform.rotation = Quaternion.RotateTowards(muzzle.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            if (Vector3.Angle(muzzle.transform.forward, direction) < 1) return true;
        }
        return false;
    }
}
