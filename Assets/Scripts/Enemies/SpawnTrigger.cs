using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    Transform player;
    void Start()
    {
        player = PlayerManager.instance.player.transform;
    }
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > 8) return;

        Spawn();
    }
    void Spawn()
    {
        foreach (GameObject enemy in enemies)
            enemy.SetActive(true);
        Destroy(gameObject);
    }
}
