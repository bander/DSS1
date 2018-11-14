using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_out : MonoBehaviour {
    Image im;
    void Start()
    {
        im = GetComponent<Image>();

        var tempColor = im.color;
        tempColor.a =1;
        im.color = tempColor;

        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {

        while (im.color.a > 0)
        {
            var tempColor = im.color;
            tempColor.a -= Time.deltaTime / 5;
            im.color = tempColor;
            yield return null;
        }
        GameObject.Destroy(gameObject);
    }

}
