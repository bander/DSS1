using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet2 : MonoBehaviour {
    public int speed = 25;
    float lifeTime = 3f;
    float time;
    public GameObject child;
    public GameObject exp;
    public Transform target;

    public AudioClip expAudio;
    public GameObject bot;

    Quaternion[] qs = new Quaternion[4];

    void Start () {
        qs[0] = Quaternion.Euler(0.9f, -0.05f, 0);
        qs[1] = Quaternion.Euler(0.85f, 0.1f, 0);
        qs[2] = Quaternion.Euler(0.6f, 0, 0);
        qs[3] = Quaternion.Euler(1f, 0.03f, 0);
	}
    public int n;
    public int frameRate = 60;
	void Update ()
    {
        qs[0] = Quaternion.Euler(0.9f*Time.deltaTime* frameRate, -0.05f * Time.deltaTime * frameRate, 0);
        qs[1] = Quaternion.Euler(0.85f, 0.1f * Time.deltaTime * frameRate, 0);
        qs[2] = Quaternion.Euler(0.6f * Time.deltaTime * frameRate, 0, 0);
        qs[3] = Quaternion.Euler(1f * Time.deltaTime * frameRate, 0.03f * Time.deltaTime * frameRate, 0);
        transform.position += transform.forward * Time.deltaTime*speed;
        
        transform.rotation *= qs[n];// Quaternion.Euler(0.9f,0,0);

        time += Time.deltaTime;
        if (time > lifeTime)
        {
            Dest();
        }

        if(target!=null)
        //if((transform.position - target.position).magnitude < 1.7f)
        if(transform.position.y <=1.5)
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
