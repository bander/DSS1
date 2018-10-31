﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraContainer : MonoBehaviour {
    public Transform target;
    public CameraPlane plane;
    public CameraControl camControl;
    public bool playerMode=true;

    //   Vector3 baseOffset = new Vector3(5.5f, 5.5f, -3);
    Vector3 baseOffset = new Vector3(5.5f,5.5f,-3);

    void Start()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime) ;// - offset;
        //plane.localPosition = -plane.up * 1.5f;
    }
    void Update()  {
        if (playerMode) transform.position = Vector3.MoveTowards(transform.position,target.position,Time.deltaTime/10);
            //transform.position = target.position ;
    }// -offset;}

    public void SetConstructMode()
    {
        camControl.target = transform;
        camControl.offset = new Vector3(5, 8, -5);
        playerMode = false;
        plane.constructMode = true;
        camControl.constructMode = true;
    }
    public void SetPlayerMode()
    {
        //camControl.target.transform.DOMove(target.position, 1).OnComplete(TransformToPlayerComplete);
        TransformToPlayerComplete();
        camControl.offset = new Vector3(5.5f, 5.5f, -3);
        playerMode = false;
        plane.constructMode = false;
    }
    void TransformToPlayerComplete()
    {
        camControl.target = target;
        camControl.constructMode = false;
    }

    public void SetLevel(int level)
    {
        plane.constructMode = false;
        camControl.offset = new Vector3(5.5f,5.5f,-3)+level* Vector3.up * 4;
        camControl.transform.DOMove(camControl.target.position + camControl.offset, 1).OnComplete(ChangeLevelComplete);
    }
    void ChangeLevelComplete()
    {
        //constructMode = true;
        plane.constructMode = true;
    }
}
