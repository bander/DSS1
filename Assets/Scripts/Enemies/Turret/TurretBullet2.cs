using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet2 : MonoBehaviour {
    int speed=45;
    float lifeTime = 1f;
    float time;
    public GameObject child;
    public GameObject exp;
    public Transform target;

    public AudioClip expAudio;
    public GameObject bot;


    void Start () {
	}
	
	void Update () {
        transform.position += transform.forward * Time.deltaTime*speed;

        time += Time.deltaTime;
        if (time > lifeTime)
        {
            Dest();
        }

        if(target!=null)
        if((transform.position - target.position).magnitude < 1.7f)
        {
            bot.GetComponent<AudioSource>().PlayOneShot(expAudio);
            Instantiate(exp,transform.position,transform.rotation);
            Dest();
        }
	}
    void Dest()
    {

        Destroy(child);
        Destroy(this);
    }
}
