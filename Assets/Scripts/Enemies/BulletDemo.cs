using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDemo : MonoBehaviour {
    GameObject target;
    GameObject impact;
    GameObject trail;
    float speed;
	void Start () {
		
	}
	public void SET(float _speed,GameObject _targ, GameObject _trail, GameObject _imp)
    {
        target = _targ;
        speed = _speed;
        impact = _imp;

        if (_trail == null) return;
        trail = Instantiate(_trail, transform.position, transform.rotation, transform);
        trail.transform.parent = transform;
    }
    float destroyTimer = 0;
	void Update () {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,target.transform.position+Vector3.up*0.3f,Time.deltaTime*speed);
            if (((target.transform.position + Vector3.up * 0.3f) - transform.position).magnitude < 0.2f)
            {
                if (impact != null)
                {
                    Instantiate(impact, transform.position, transform.rotation, transform);
                }
                dest();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,transform.position + transform.forward, Time.deltaTime * speed);

            destroyTimer += Time.deltaTime;
            if (destroyTimer > 2)
            {
                dest();
            }
        }
	}
    bool destinated = false;
    void dest()
    {
        if(destinated) return;
        destinated = true;
            
        target.GetComponent<EnemyDemoScene>().Death();

        GameObject.Destroy(trail, 0.5f);
                GameObject.Destroy(this.gameObject,0.5f);

    }
}
