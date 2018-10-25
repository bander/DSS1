using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {
    public Transform target;
    public Vector3 offsetPlayer;
    public Vector3 offsetConstruct;
    Vector3 newOffset;
    Vector3 offset;

    int speed = 10;

    delegate void UpdateCamera();
    UpdateCamera updateCamera;

    Vector3 startPosition;

    void Start()
    {
        updateCamera = PlayerMode;
        updateCamera += CorrectDistance;
        newOffset = offsetPlayer;
        //newOffset.//.DOMove(offsetPlayer, 1);
        
    }

	void LateUpdate () {
        if (updateCamera != null) updateCamera.Invoke();
	}
    void PlayerMode()
    {
        transform.position = target.position - offset;
        transform.LookAt(target.position - Vector3.up);
    }
    void ConstructMode()
    {
        transform.position = target.position - offset;
        transform.LookAt(target.position - Vector3.up);
    }

    public void SetCameraMode(int num=0)
    {
        switch (num)
        {
            case 0:
                updateCamera = PlayerMode;
                updateCamera += CorrectDistance;
                newOffset = offsetPlayer;
                break;
            case 1:
                updateCamera = ConstructMode;
                updateCamera += CorrectDistance;
                startPosition = transform.position;
                newOffset = offsetConstruct;
                break;
        }
    }
    void CorrectDistance()
    {
        offset = Vector3.MoveTowards(offset, newOffset, Time.deltaTime * speed);
    }
}
