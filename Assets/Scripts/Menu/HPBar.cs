using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour {
    public GameObject text;
    CharacterStats stats;
    Slider slider;

	void Start () {
        slider = GetComponent<Slider>();
	}

    public void UpdateBar(CharacterStats newStats) {
        stats = newStats;

        slider.value = ((float)stats.currentHealth / (float)stats.maxHealth);
        updateText();
	}

    void updateText()
    {
        if (stats != null)
        {
            text.GetComponent<TMP_Text>().text = stats.currentHealth + "/" + stats.maxHealth;
        }
    }

    public void Show(bool act=true)
    {
        transform.parent.gameObject.SetActive(act);
    }
}
