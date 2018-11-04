using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashTester : MonoBehaviour {
    
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveHome.Instance.v.Add(new Vector3(Random.Range(0,5),Random.Range(0,5),0));
            SaveHome.Save();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveHome.Load();
            List<Vector3> dd = SaveHome.Instance.v;
            int i = 0;
            foreach (Vector3 v3 in dd)
            {
                Debug.Log(i + " Load " + v3);
                i++;
            }
        }
    }
}
