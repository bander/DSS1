using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCamCube : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;
    Vector3 prevposition;

    delegate void MB();
    MB mb;

    void OnMouseDragX()
    {
        RaycastHit hit;
        Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out hit, 100f, LayerMask.GetMask("CamCube")))
        {
            Vector3 delta = hit.point - prevposition; 
            target.position += delta/2;
            prevposition = hit.point;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100f, LayerMask.GetMask("CamCube")))
            {
                prevposition = hit.point;
                mb = OnMouseDragX;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            mb = null;
        }
        if (mb != null) mb.Invoke();
        transform.rotation = Quaternion.identity;
    }
}
