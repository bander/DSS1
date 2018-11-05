using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartSceneController : MonoBehaviour {
    Animator anim;

    public GameObject plane;
    public Transform head;
    public Transform cam;

    Vector3 offset = new Vector3(5.5f, 5.5f, -3);

    public Transform go;
    Ease eas = Ease.InOutCubic;
	void Start ()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
     //   Screen.fullScreen = true;
      //  Cursor.visible = false;
        StartShow();
    }
    void StartShow()
    {
        cam.transform.DOMoveY(4, 3).SetEase(eas);
        wasStarted = true;

    }
    bool wasStarted = false;
    float p1 = 0.1f;
    float a1 = 0.5f;
    float a3 = 4f;
    float a4 = 5f;
    float c2 = 3f;

    float camSpeed = 5;
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartShow();
        }
            if (!wasStarted) return;

        go.LookAt(head.transform);
        cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation,go.rotation,Time.deltaTime*camSpeed);
        c2 -= Time.deltaTime;
        a1 -= Time.deltaTime;
        a3 -= Time.deltaTime;
        a4 -= Time.deltaTime;
        p1 -= Time.deltaTime;
        
        if (c2 <= 0)
        {
            c2 = 1000000;
            //cam.transform.DOLocalMoveX(cam.transform.position.x -9, 6).SetEase(eas);
            cam.transform.DOMove(cam.transform.position +new Vector3(9,0,-4), 6).SetEase(eas);
            //cam.transform.DOLocalMoveX(-18, 4).SetEase(Ease.InOutCirc);
            //           cam.transform.DOMove(transform.position+offset, 4).SetEase(eas);
        }
        if (a1 <= 0)
        {
            a1 = 10000000;
            anim.speed = 1;
        }
        if (a3 <= 0)
        {
            a3 = 10000000;
            camSpeed = 35;
        }
        if (a4 <= 0)
        {
            a4 = 10000000;
    //        camSpeed = 45;
        }
        if (p1 <= 0)
        {
            p1 = 10000000;
            plane.transform.DOMoveY(-0.1f, 2);//.SetEase(Ease.InOutCirc);
        }

        //cam.transform.LookAt(head);
        if (Input.GetKeyDown(KeyCode.Q))
        {
        //    StartShow();
         //   plane.transform.DOMoveY(-0.1f, 3).SetEase(Ease.InOutCirc);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
 //           cam.transform.DOLocalMoveY(5, 6);
            offset.y = 5;
//            cam.transform.DOMove(transform.position + offset, 6).SetEase(eas);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //cam.transform.DOLocalMoveX(-22, 7);// (-18, 4);
            offset.x = -22;
            cam.transform.DOMove(transform.position + offset, 7).SetEase(eas);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.speed = 1;
        }
	}
}
