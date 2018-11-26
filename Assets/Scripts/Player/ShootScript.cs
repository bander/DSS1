using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour {
    AudioSource audio;
    PlayerStats stats;
    Equipment weapon;
    MenuScript menu;

    AudioClip clip;
    GameObject effect;
    GameObject bullet;
    Transform muzzle;
    float fireDist;

    public Transform muzzlePistol;
    public Transform muzzleMachine;

    void Start() {
        if(GameObject.FindGameObjectWithTag("Menu")!=null)
            menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuScript>();

        audio = GetComponent<AudioSource>();
        stats = GetComponent<PlayerStats>();
        stats.onStatsUpdate += UpdateWeapon;
        UpdateWeapon();
    }
    void UpdateWeapon()
    {
        weapon = stats.Wepaon;
        if (weapon != null)
        {
            clip = weapon.audioClip;
            effect = weapon.muzzleEffect;
            fireDist = weapon.attackDistance;
            bullet = weapon.bullet;

            switch (weapon.attackType)
            {
                case WeaponAttackType.Pistol:
                    muzzle = muzzlePistol;
                    break;
                case WeaponAttackType.Automative:
                    muzzle = muzzleMachine;
                    break;
            }
        }
        else
        {
            fireDist = 0.7f;
            clip = null;
            effect = null;
            bullet = null;
        }


    }

    public GameObject trash;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*GameObject mz=Instantiate(trash, muzzlePistol.transform.position, muzzlePistol.transform.rotation);
            mz.transform.parent = muzzlePistol.transform;
            //*/
        }

    }
    public void Fire(GameObject enemy)
    {
        if (effect != null)
        {
            GameObject mz = Instantiate(effect, muzzlePistol.transform.position, muzzlePistol.transform.rotation);
            mz.transform.parent = muzzlePistol.transform;
        }
        if (bullet != null)
        {
            GameObject bl = Instantiate(bullet, muzzlePistol.transform.position, muzzlePistol.transform.rotation);
            bl.GetComponent<PistolBullet>().target = enemy;
            //mz.transform.parent = muzzlePistol.transform;
        }

        if (clip!=null)
            audio.PlayOneShot(clip);

        if (enemy != null && CheckDistToEnemy(enemy))
        {
            if(enemy.GetComponent<EnemyStats>()!=null)
                enemy.GetComponent<EnemyStats>().TakeDamage(GetComponent<PlayerStats>().damage.GetValue());
            if (enemy.GetComponent<EnemyStatsWorm>() != null)
                enemy.GetComponent<EnemyStatsWorm>().TakeDamage(GetComponent<PlayerStats>().damage.GetValue());
        }
            
        
    } 
    bool CheckDistToEnemy(GameObject enemy)
    {
        float dist = (transform.position - enemy.transform.position).magnitude;
        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        if (dist < (fireDist+0.5f) && !enemyStats.dead) return true;
        return false;
    }
}
