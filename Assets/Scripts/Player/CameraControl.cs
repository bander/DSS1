using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoBehaviour {
    public Transform target;
    public Vector3 offset = new Vector3(5.5f, 7f, -3);
    public CameraPlane plane;
    public bool constructMode = false;

    public Transform rotator;

    void Start()
    {
        transform.position = target.position + offset;
        rotator.LookAt(target.transform);
        transform.rotation = rotator.rotation;
    }
    void Update()  {
        transform.position = Vector3.MoveTowards(transform.position, target.position + offset, Time.deltaTime * 10);// - offset;

        if (!constructMode)
        {
            rotator.LookAt(target.transform);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotator.rotation, Time.deltaTime * 215);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(-offset.x,-offset.y,-offset.z)), Time.deltaTime * 40);
        }
    }
    void ChangeLevelComplete()
    {
        constructMode = true;
        plane.constructMode = true;
    }
}
