using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour {
    Animator anim;
    PlayerController pController;
    void Awake()
    {
        anim = GetComponent<Animator>();
        pController = GetComponent<PlayerController>();
    }
    public void IdleStarting()
    {
        anim.SetTrigger("IdleStarting");
    }
    public void ShootEvent()
    {
        pController.Shoot();
    } 
}
