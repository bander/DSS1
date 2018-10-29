using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBullet : MonoBehaviour {
    int speed=25;
    public float lifeTime = 1.5f;
    float time;
    public GameObject child;
    public GameObject exp;
    public Transform target;

    public AudioClip expAudio;
    public GameObject bot;


    void Start () {
	}
	
	void Update () {
        transform.position -= transform.right * Time.deltaTime*speed;

        time += Time.deltaTime;
        if (time > lifeTime)
        {
            Dest();
        }

        if(target!=null)
        if((transform.position - target.position).magnitude < 2.2)
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
