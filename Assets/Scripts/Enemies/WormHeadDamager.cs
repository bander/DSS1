using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHeadDamager : MonoBehaviour
{
    public float dist;
    Transform player;

    [SerializeField]
    Transform spine;

    float lastDamageTime;
    float damageReset = 2;

    void Start()
    {
        player = PlayerManager.instance.player.transform;
    }


    void Update()
    {
        dist = Vector3.Distance(player.position, transform.position);
        float dist2 = Vector3.Distance(player.position, spine.position);
        if (dist < 2f || dist2<2f)
            if (Time.realtimeSinceStartup > lastDamageTime+2)
                Damage();
    }
    void Damage()
    {
        lastDamageTime = Time.realtimeSinceStartup;
        PlayerManager.instance.player.GetComponent<PlayerStats>().TakeDamage(25);
    }
}
