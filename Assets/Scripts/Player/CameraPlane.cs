﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPlane : MonoBehaviour {
    public Camera mainCamera;
    public Transform fakeTarget;
    Vector3 prevposition;

    public bool constructMode=false;

    delegate void MB();
    MB mb;

    void OnMouseDragX()
    {
        if (!constructMode)
            return;

        RaycastHit hit;
        Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out hit, 10000f, LayerMask.GetMask("CamCube")))
        {
            Vector3 delta = prevposition- hit.point;
            fakeTarget.position += delta /10;
            prevposition = hit.point;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            mb = null;
        }
        if (mb != null) mb.Invoke();


       // if (!constructMode)
            transform.rotation = Quaternion.identity;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (!constructMode) return;
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 10000f, LayerMask.GetMask("CamCube")))
            {
                prevposition = hit.point;
                mb = OnMouseDragX;
            }
        }
    }
}
