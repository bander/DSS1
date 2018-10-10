using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerA : MonoBehaviour {
    public Transform target;
    public Vector3 offset1;

	void LateUpdate () {
        transform.position = target.position - offset1;
        transform.LookAt(target.position);// -Vector3.up);
	}
}
