using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    public GameObject head;
    public GameObject target;
    public Transform[] muzzles;

    float speedRotation = 120;
    float rotDuration;
    public AudioClip rotAudio;
    public AudioClip shotAudio;
    float timerAudio=2;
    float fireInterval = 1.3f;
    float timerShot=2;

    bool readyToShor = false;

    public GameObject muzzle;
    public GameObject bullet;
    public GameObject bullet2;
    public GameObject trail;
    public GameObject impact;

    void Start () {

	}
    bool trash = true;
	void Update () {;

        if (Input.GetKeyDown(KeyCode.F))
        {
            trash = false;
        }
        if (trash) return;

        Quaternion dop = transform.rotation;
        dop.y = -dop.y;
        Quaternion q = Quaternion.LookRotation( target.transform.position - head.transform.position, transform.up) * Quaternion.Euler(0,-90,0);
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
        else
        {
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
        bulletx.transform.LookAt(target.transform.position+Vector3.up);
        bulletx.transform.rotation = bulletx.transform.rotation * Quaternion.Euler(0, 90, 0);


        trailx.transform.parent = bulletx.transform;
        bulletx.GetComponent<TurretBullet>().child = trailx;
        bulletx.GetComponent<TurretBullet>().exp = impact;
        bulletx.GetComponent<TurretBullet>().target = target.transform;
        bulletx.GetComponent<TurretBullet>().bot = this.gameObject;
    }
}
