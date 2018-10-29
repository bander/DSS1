using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAnimator1 : MonoBehaviour {
    public Transform target;
    public Transform targetShoot;
    NavMeshAgent agent;
    Animator anim;

    AudioSource source;
    public AudioClip[] steps;
    public AudioClip shots;
    bool currentStep;
    bool currentShot;

    public GameObject muzzles;
    public GameObject impacts;

    public Transform[] muzzleTransform;

    float fireInterval = 1.5f;
    float fireInterval2 = 0.1f;
    float timeFire;
    float timeFire2;
    int shotsInTime = 7;
    int shotsIn;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        source =GetComponent<AudioSource>();

        agent.SetDestination(target.position);
        agent.updateRotation = false;
        
	}
	
	void Update () {
        transform.LookAt(targetShoot);

        Vector3 velocity = agent.desiredVelocity;
        bool animMoved = (velocity.magnitude > 0.1f)? true : false;
        float angle = Vector3.Angle(transform.forward, velocity);
        
        anim.SetBool("isMoved",animMoved);
        anim.SetFloat("Angle", angle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
        timeFire += Time.deltaTime;
        if (timeFire>fireInterval)
        {
            timeFire = 0;
            ShotPre();
            //Shot();
        }
        timeFire2 += Time.deltaTime;
        if(shotsIn<shotsInTime)
        if (timeFire2 > fireInterval2 )
        {
            timeFire2 = 0;
            Shot();
        }

        muzzleTransform[0].LookAt(targetShoot);
        Quaternion q = muzzleTransform[0].rotation * Quaternion.Euler(13-shotsIn*2.5f, 0, 0);
        Vector3 dir = q*Vector3.forward;
        //Debug.DrawLine(muzzleTransform[0].position, muzzleTransform[0].position + dir* 10);
    }

    public void _AudioStep()
    {
        currentStep = !currentStep;
        int step = (currentStep) ? 0 : 1;
        source.PlayOneShot(steps[step]);
    }

    void ShotPre()
    {
        currentShot = !currentShot;
        shotsIn = 0;
    }
    void Shot()
    {
        shotsIn++;
        int n = (currentShot) ? 0 : 1;
        Instantiate(muzzles, muzzleTransform[n].position, muzzleTransform[n].rotation);


        muzzleTransform[0].LookAt(targetShoot);
        Quaternion q = muzzleTransform[0].rotation * Quaternion.Euler(9 - shotsIn * 1.5f, 0, 0);
        Vector3 dir = q * Vector3.forward;
        //Debug.DrawLine(muzzleTransform[0].position, muzzleTransform[0].position + dir * 10);
        Ray r = new Ray(muzzleTransform[0].position,dir);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 100))
        {
            Instantiate(impacts, hit.point, Quaternion.Euler(-90, 0, 0));
        }


        /*
        GameObject trail=Instantiate(trails, muzzleTransform[n].position, muzzleTransform[n].rotation);
        GameObject bullet = Instantiate(bulletPrefab, muzzleTransform[n].position, muzzleTransform[n].rotation);
        trail.transform.parent = bullet.transform;
        bullet.GetComponent<BotBullet>().child = trail;
        bullet.GetComponent<BotBullet>().exp = impacts[0];
        bullet.GetComponent<BotBullet>().target = targetShoot;
        bullet.GetComponent<BotBullet>().bot = this.gameObject;
        //*/
        _AudioShot();
    }
    public void _AudioShot()
    {
        int step = (currentShot) ? 0 : 1;
        source.PlayOneShot(shots);
    }
}
