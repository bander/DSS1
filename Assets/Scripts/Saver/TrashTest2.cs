using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTest2 : MonoBehaviour {
    public Item it;
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameDataController.GetItems();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //GameDataController.SetItem((int)Random.Range(0, 7), new Vector3(0,0,0));
        }
    }
}
