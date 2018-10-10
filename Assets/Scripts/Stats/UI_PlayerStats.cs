using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PlayerStats : MonoBehaviour {
    public GameObject[] textFields;
    PlayerStats stats;

	void Start () {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.onStatsUpdate += UpdateStats;
	}
	
    void UpdateStats()
    {
        textFields[0].GetComponent<TMP_Text>().text = stats.damage.GetValue().ToString();
        textFields[1].GetComponent<TMP_Text>().text = stats.attackRate.GetValue().ToString();
        textFields[2].GetComponent<TMP_Text>().text = stats.attackDist.GetValue().ToString();
        textFields[3].GetComponent<TMP_Text>().text = stats.armor.GetValue().ToString();
        textFields[4].GetComponent<TMP_Text>().text = stats.speed.GetValue().ToString();
    }
}
