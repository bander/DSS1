using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraContainer : MonoBehaviour {
    public Transform target;
    public CameraPlane plane;
    public CameraControl camControl;
    public bool playerMode=true;

    //   Vector3 baseOffset = new Vector3(5.5f, 5.5f, -3);
    public Vector3 baseOffset = new Vector3(5.5f,7f,-3);
    public Vector3 constructOffset = new Vector3(5,11,-3);

    void Start()
    {
        camControl.offset = baseOffset;
        camControl.offset = baseOffset;
        playerMode = true;
        plane.constructMode = false;
        camControl.target = target;
        camControl.constructMode = false;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime*10) ;
    }
    void Update()
    {
        if (playerMode) 
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 10);
    }

    public void SetConstructMode()
    {
        camControl.target = transform;
        playerMode = false;
        plane.constructMode = true;
        camControl.constructMode = true;
        camControl.offset = constructOffset;
    }
    public void SetPlayerMode()
    {
        //camControl.target.transform.DOMove(target.position, 1).OnComplete(TransformToPlayerComplete);
  //      TransformToPlayerComplete();
        camControl.offset = baseOffset;
        playerMode = true;
        plane.constructMode = false;
 //   }
 //   void TransformToPlayerComplete()
 //   {
        camControl.target = target;
        camControl.constructMode = false;
    }

    public void SetLevel(int level)
    {
        plane.constructMode = false;
        camControl.offset = constructOffset+level* Vector3.up * 4;
        camControl.transform.DOMove(camControl.target.position + camControl.offset, 1).OnComplete(ChangeLevelComplete);
    }
    void ChangeLevelComplete()
    {
        plane.constructMode = true;
    }
}