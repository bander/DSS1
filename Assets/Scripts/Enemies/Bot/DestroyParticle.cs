using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {
    public float lifeTime;
    void Start () {
        if (lifeTime==0)
            Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
        else
            Destroy(gameObject, lifeTime);
    }
	
}
