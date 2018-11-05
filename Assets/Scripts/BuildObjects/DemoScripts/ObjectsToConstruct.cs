using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Inventory/Object")]
public class ObjectsToConstruct :ScriptableObject {

    new public GameObject go;
    new public Vector3 pos;
    new public Vector3 rot;
    new public Vector3 scal;
    new public Quaternion rot2;

    void Start () {
		
	}
	
}
