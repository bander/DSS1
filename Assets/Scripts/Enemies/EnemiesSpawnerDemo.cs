using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnerDemo : MonoBehaviour {

    public GameObject[] enemies;
    void OnTriggerEnter(Collider other) { 

        foreach (GameObject go in enemies)
        {
            if(go!=null)
                go.SetActive(true);
        }
        Destroy(gameObject);
	}
	
}
