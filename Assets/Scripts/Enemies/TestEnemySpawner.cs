using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject[] enemies;
    [SerializeField] float spawnInterval;

    bool active;
    float timer;
    public void ActivateSpawn(bool act=true)
    {
        timer = 0;
        active = act;
    }

	void Update () {
        if (!active) return;

        timer += Time.deltaTime;
        if (timer > spawnInterval)
        {
            timer = 0;
            SpawnWave();
        }
	}
    void SpawnWave()
    {
        foreach (GameObject en in enemies)
        {
            GameObject enemy = GameObject.Instantiate(en, en.transform.position, en.transform.rotation);
            enemy.SetActive(true);
        }
    }
}
