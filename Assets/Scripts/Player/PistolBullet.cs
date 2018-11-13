using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour {
    public float speed = 20;
    public GameObject target;

	void Update () {
        Vector3 tPos = target.transform.position;
        if (target.GetComponent<EnemySampleWorm>())
        {
           tPos += Vector3.up;
        }
        if (target.GetComponent<EnemySample>())
        {
            if (target.GetComponent<EnemySample>().enemyName == "Small")
            {
                tPos += Vector3.up*1.4f;
            }
            if (target.GetComponent<EnemySample>().enemyName == "Big")
                tPos += Vector3.up * 3;
        }
        

        Vector3 dir = tPos - transform.position;
        transform.position = Vector3.MoveTowards(transform.position,
                      tPos, Time.deltaTime*speed);
        if (dir.magnitude < 1)
        {
            GameObject.Destroy(this, 1);
        }
	}
}
