using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLeg : MonoBehaviour {

    public delegate void OnIntersect();
    public OnIntersect onIntersect;

    Collider player;
    Collider coll;

    float timer;
    float damageDelay = 0.5f;

	void Start () {
        player = PlayerManager.instance.player.GetComponent<Collider>();
        coll = GetComponent<Collider>();
	}

	void Update() {
        timer += Time.deltaTime;

        if (timer < damageDelay) return;

        if (coll!=null && player!=null)
        {
            if (coll.bounds.Intersects(player.bounds))
            {
                timer = 0;
                if (onIntersect != null) onIntersect.Invoke();
            }
        }
    }
}
