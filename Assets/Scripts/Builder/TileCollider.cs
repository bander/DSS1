using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollider : MonoBehaviour {
    public List<Collider> cols;
    
    void OnTriggerEnter(Collider coll)
    {
        cols.Add(coll);
    }
    void OnTriggerExit(Collider coll)
    {
        cols.Remove(coll);
    }
}
