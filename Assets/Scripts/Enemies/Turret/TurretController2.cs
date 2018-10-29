using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController2 : MonoBehaviour {
    public GameObject head;
    public GameObject holder;
    public GameObject target;
    public Transform[] muzzles;

    float speedRotation = 170;
    float speedRotation2 = 60;
    float rotDuration;
    public AudioClip rotAudio;
    public AudioClip shotAudio;
    float timerAudio=2;
    float fireInterval = 0.3f;
    float timerShot=0.3f;
    bool wasShoot = false;
    int shots = 0;

    bool readyToShor = false;

    public GameObject muzzle;
    public GameObject bullet;
    public GameObject bullet2;
    public GameObject trail;
    public GameObject impact;

    void Start () {

	}
    bool act = false;
	void Update () {;

        if (Input.GetKeyDown(KeyCode.K))
        {
            wasShoot = false;
            act = true;
        }

        if (!act) return;
        Quaternion dop = transform.rotation;
        dop.y = -dop.y;
        Quaternion q2 = Quaternion.Euler(-30,0,0);
        Quaternion q = Quaternion.LookRotation( target.transform.position - head.transform.position, transform.up) * Quaternion.Euler(-90,0,0);
        //q.y = -q.y;
        head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation,q , Time.deltaTime * speedRotation);
        
        if (Quaternion.Angle(head.transform.rotation, q)>1)
        {
            readyToShor = false;
            timerAudio += Time.deltaTime;
            if (timerAudio > rotAudio.length)
            {
                timerAudio = 0;
                playRotAudio();
            }
        }
        else if (Quaternion.Angle(holder.transform.localRotation, q2) > 1)
        {

            holder.transform.localRotation = Quaternion.RotateTowards(holder.transform.localRotation, q2, Time.deltaTime * speedRotation2);

            readyToShor = false;
            timerAudio += Time.deltaTime;
            if (timerAudio > rotAudio.length)
            {
                timerAudio = 0;
                playRotAudio();
            }
        }
        else
        {
            if (wasShoot) return;

            readyToShor = true;
            timerAudio = 2;
            timerShot += Time.deltaTime;
            if (timerShot>=fireInterval)
            {
                timerShot = 0;
                Shot();
            }
        }
    }
    void playRotAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(rotAudio);
    }
    bool currentShot;
    void Shot()
    {
        GetComponent<AudioSource>().PlayOneShot(shotAudio);

        currentShot = !currentShot;
        int n = (currentShot) ? 0 : 1;

        Instantiate(muzzle, muzzles[n].position, muzzles[n].rotation);


        GameObject trailx = Instantiate(trail, muzzles[n].position, muzzles[n].rotation);
        GameObject bulletx = Instantiate(bullet, muzzles[n].position, muzzles[n].rotation);
        bulletx.transform.rotation = bulletx.transform.rotation * Quaternion.Euler(90, 0, 0);
        //bulletx.transform.LookAt(target.transform);


        trailx.transform.parent = bulletx.transform;
        bulletx.GetComponent<TurretBullet2>().child = trailx;
        bulletx.GetComponent<TurretBullet2>().exp = impact;
        bulletx.GetComponent<TurretBullet2>().target = target.transform;
        bulletx.GetComponent<TurretBullet2>().bot = this.gameObject;
        bulletx.GetComponent<TurretBullet2>().n = shots;

        shots++;
        if (shots >= 4)
        {
            wasShoot = true;
        }
    }
}
