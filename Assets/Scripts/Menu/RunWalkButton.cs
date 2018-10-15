using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunWalkButton : MonoBehaviour {
    PlayerControl player;
	void Start () {
        player = PlayerManager.instance.player.GetComponent<PlayerControl>();
	}
	
    public void Click()
    {
        player.Crouch = !player.Crouch;
    }
}
