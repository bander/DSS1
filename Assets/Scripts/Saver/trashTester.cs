using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashTester : MonoBehaviour {
    int num = 0;
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //SaveController1.instance.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //SaveController1.instance.LoadData();
        }
    }
}
