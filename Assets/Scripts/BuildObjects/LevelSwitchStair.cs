using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitchStair : MonoBehaviour {
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("ff "+other.transform.position);
            HomeConstructor.instance.ChangeLevelByPlayerPosition(other.transform.position);
        }
    }
}
