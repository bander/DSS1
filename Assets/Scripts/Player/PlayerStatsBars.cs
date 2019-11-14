using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBars : MonoBehaviour {

    #region Singleton
    public static PlayerStatsBars instance;

    void Awake()
    {
        instance = this;
    }

    #endregion 


    PlayerStats pStats;
    public Image hpBar;
    public Image oxygenBar;
    public Image energyBar;

    void Start()
    {
        pStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void UpdateStats()
    {
        if(pStats==null) pStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>(); ;
        
        hpBar.fillAmount = pStats.currentHealth / pStats.maxHealth;
        oxygenBar.fillAmount = pStats.currentOxygen / pStats.maxOxygen;
        energyBar.fillAmount = pStats.currentEnergy/pStats.maxEnergy;
    }
}
