using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trashh : MonoBehaviour {
    public NavMeshAgent a1;
    public NavMeshAgent a2;
    bool is1;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.T))
        {
            is1 = !is1;
            if (is1)
            {
                a1.gameObject.SetActive(true);
                a2.gameObject.SetActive(false);
            }
            else
            {
                a1.gameObject.SetActive(false);
                a2.gameObject.SetActive(true);
            }
        }
	}
}
