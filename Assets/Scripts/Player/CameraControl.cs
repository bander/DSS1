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

    bool startScene = false;
    public GameObject planeDark;

    void Start()
    {
        transform.position = target.position + offset;
        rotator.position = transform.position;
        rotator.LookAt(target.transform);
        transform.rotation = rotator.rotation;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().CrossFade("wakeup_motherfucker", 0.001f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().Update(0);

        if (startScene)
            StartSceneAnimation();
    }
    void StartSceneAnimation()
    {
        transform.position = target.position +Vector3.up;

        rotator.position = transform.position;
        rotator.LookAt(target.transform);
        transform.rotation = rotator.rotation * Quaternion.Euler(0, 0, 90);

        transform.position = transform.position-(target.transform.forward*0.5f+ Vector3.up);
        //transform.DORotate( Quaternion.Euler(0,0,0)*transform.forward, 3);
        planeDark.transform.DOMove(planeDark.transform.position - Vector3.up * 2, 2).SetDelay(2);
        transform.DOMove(transform.position + (target.transform.forward * 4 + Vector3.up*2), 14).SetEase(Ease.InOutSine);
    }
    void Update()  {
        transform.position = Vector3.MoveTowards(transform.position, target.position + offset, Time.deltaTime * 10);// - offset;
        

        if (!constructMode)
        {
            rotator.position = transform.position;

            rotator.LookAt(target.transform);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotator.rotation, Time.deltaTime * 215);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotator.rotation, Time.deltaTime * 15);

        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(-offset.x,-offset.y,-offset.z)), Time.deltaTime * 40);
 //           transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(-offset.x,-offset.y,-offset.z)), Time.deltaTime * 10);
        }
    }
    void ChangeLevelComplete()
    {
        constructMode = true;
        plane.constructMode = true;
    }
}
