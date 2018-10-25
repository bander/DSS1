using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashCam : MonoBehaviour {
    public Vector3 offset=new Vector3(5,5,-5);
    public Transform target;
    
    void Start()
    {
        transform.position = offset;

    }

	void Update () {
        transform.LookAt(target.position - Vector3.up);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            offset.y = offset.y - 4;
            SetLevel();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            offset.y = offset.y + 4;
            SetLevel();
        }
    }
    void SetLevel()
    {
        transform.DOMove(offset,1);
        //target.position = target.position + target.up * 4 * z;
    }
}
