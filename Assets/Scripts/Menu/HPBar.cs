using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour {
    public GameObject text;
    public int maxHP=100;
    public int currentHP=100;
    Slider slider;

	void Start () {
        slider = GetComponent<Slider>();
        currentHP = maxHP;
       slider.value = ((float)currentHP / (float)maxHP);
        updateText();
	}

    public void UpdateBar(int value) {
        currentHP = value;
        slider.value = ((float)currentHP / (float)maxHP);
        updateText();
	}

    void updateText()
    {
        text.GetComponent<TMP_Text>().text = currentHP+"/" +maxHP;
    }

    public void Show(bool act=true)
    {
        transform.parent.gameObject.SetActive(act);
    }
}
