using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class TimedObjectAlpha : MonoBehaviour {
    
	void Start () {
        transform.DOMoveY(transform.position.y + 100, 1.5f);
        GetComponent<TextMeshProUGUI>().DOFade(0, 1).SetDelay(0.5f).OnComplete(this.CompleteDestroy);
	}
    void CompleteDestroy()
    {
        Destroy(gameObject);
    }
}
