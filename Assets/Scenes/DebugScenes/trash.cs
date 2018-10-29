using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class trash : MonoBehaviour {
    public Vector3 dest = new Vector3(4, 0, 0);
	// Use this for initialization
	void Start () {
        GetComponent<NavMeshAgent>().SetDestination(dest);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
