using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoBehaviour {
    public Transform target;
    public Vector3 offset = new Vector3(5, 5, -5);
    public CameraPlane plane;
    public bool constructMode = false;

    void Start()
    {
        transform.position = target.position + offset;
        //plane.transform.position = target.position - transform.up * 1.1f;
    }
    void Update()  {

        if (!constructMode)
        {
            transform.position = target.position + offset;
        }
        transform.LookAt(target.transform);
        //transform.localPosition = offset;

        if (Input.GetKeyDown(KeyCode.G))
        {
            constructMode = false;
            plane.constructMode = false;
            offset += Vector3.up * 4;
            transform.DOMove(target.position+offset, 1).OnComplete(ChangeLevelComplete);
        }
    }
    void ChangeLevelComplete()
    {
        constructMode = true;
        plane.constructMode = true;
    }
}
