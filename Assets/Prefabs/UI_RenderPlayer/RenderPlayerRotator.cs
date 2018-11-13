using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPlayerRotator : MonoBehaviour {
    public float speed;
    float currentRotation = 0;
	void Update () {
        currentRotation += speed;
        transform.rotation = Quaternion.Euler(0,currentRotation,0);
	}
}
