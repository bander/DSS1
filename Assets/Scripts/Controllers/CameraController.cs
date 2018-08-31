using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public Vector3 offset;

	void LateUpdate () {
        transform.position = target.position - offset;
        transform.LookAt(target.position-Vector3.up);
	}
}
