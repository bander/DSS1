﻿using UnityEngine;

public class Interactable : MonoBehaviour {
    Transform player;
    public float radius = 2f;
    bool isInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        if (player != null)
        {
            if ((player.position - transform.position).magnitude < radius)
            {
                if (!isInRange)
                {
                    isInRange = true;
                    inRange();
                }
            }
            else
            {
                if (isInRange)
                {
                    isInRange = false;
                    MenuScript.instance.removeImteractables(this);
                }

            }
        }
        else
        {
           // Debug.Log("null "+name  );
        }
    }

    public virtual void Interact()
    {

    }

    public virtual void inRange()
    {
        MenuScript.instance.addImteractables(this);
    }
}
