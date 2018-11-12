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
    Transform muzzle;
    float fireDist;

    public Transform muzzlePistol;
    public Transform muzzleMachine;

    void Start() {
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
        }


    }
    

    public void Fire(GameObject enemy)
    {
        if (effect != null)
            Instantiate(effect, muzzle.transform);

        if(clip!=null)
            audio.PlayOneShot(clip);

        if (enemy != null && CheckDistToEnemy(enemy))
            enemy.GetComponent<EnemyStats>().TakeDamage(GetComponent<PlayerStats>().damage.GetValue());
        
    } 
    bool CheckDistToEnemy(GameObject enemy)
    {
        float dist = (transform.position - enemy.transform.position).magnitude;
        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        if (dist < (fireDist+0.5f) && !enemyStats.dead) return true;
        return false;
    }
}
