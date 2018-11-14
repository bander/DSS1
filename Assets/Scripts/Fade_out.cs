using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_out : MonoBehaviour {
    void Start()
    {
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        Image im = GetComponent<Image>();

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
