using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAnimator1 : MonoBehaviour {
    public Transform target;
    public Transform targetShoot;
    NavMeshAgent agent;
    Animator anim;

    public AudioSource source;
    public AudioClip[] steps;
    public AudioClip shots;
    bool currentStep;
    bool currentShot;

    public GameObject muzzles;
    public GameObject impacts;

    public Transform[] muzzleTransform;

    float fireInterval = 0.2f;
    float timeFire;

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
            Shot();
        }
	}

    public void _AudioStep()
    {
        currentStep = !currentStep;
        int step = (currentStep) ? 0 : 1;
        source.PlayOneShot(steps[step]);
    }

    void Shot()
    {
        currentShot = !currentShot;
        int n = (currentShot) ? 0 : 1;
        Instantiate(muzzles, muzzleTransform[n].position, muzzleTransform[n].rotation);
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
