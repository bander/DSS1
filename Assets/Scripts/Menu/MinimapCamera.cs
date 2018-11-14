using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {
    Transform target;
    void Start()
    {
        target = PlayerManager.instance.player.transform;
    }
	void Update () {
        transform.position = target.position;

	}
}
