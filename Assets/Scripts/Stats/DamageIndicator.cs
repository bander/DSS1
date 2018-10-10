using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour {
    public GameObject prefab;
	void Start () {
		
	}

    public void showIndicator(string text, Vector3 position)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(position);
        pos.x += Random.Range(-40, 40);
        GameObject ob = Instantiate(prefab,pos,Quaternion.identity);
        ob.GetComponent<TMP_Text>().text = text;
        ob.transform.parent = transform;
    }
}
