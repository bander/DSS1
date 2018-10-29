using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCam2 : MonoBehaviour {
    public Transform target;
    public Transform cube;
    public Vector3 offset = new Vector3(-5, -5, 5);

    void Start ()
    {
        transform.position = target.position - offset;
        cube.transform.position = target.position-transform.up*1.1f;
    }
	void Update () {
        transform.position = target.position - offset;
        //transform.LookAt(target.position - Vector3.up);
    }
}
