using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    #region Singleton
    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion 

    PlayerStats pStats;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pStats = player.GetComponent<PlayerStats>();

        /*
        SaveGame.Load();
        Vector3 savedStats = SaveGame.Instance.s;
        pStats.currentHealth = savedStats.x;
        pStats.currentOxygen = savedStats.y;
        pStats.currentEnergy = savedStats.z;
        //*/

        if(PlayerStatsBars.instance!=null)
            PlayerStatsBars.instance.UpdateStats();

        playerUpdate += oxygenDecrease;
        playerUpdate += energyDecrease;
    }

    delegate void PlayerUpdate();
    PlayerUpdate playerUpdate;

    public GameObject player;
    public bool demo = true;


    float hpEncreaseInterval;
    float hpEncreaseIntervalLow = 7;
    float hpEncreaseIntervalHight = 3;
    float energyDecreaseInterval = 2;//36;
    float oxygenDecreaseInterval = 2;//18

    float hpTimer;
    float energyTimer;
    float oxygenTimer;

    void Update()
    {
        if (playerUpdate != null) playerUpdate.Invoke();
    }
    void hpStartEncrease()
    {

    }
    void hpEncrease()
    {
        hpTimer += Time.deltaTime;
        if (hpTimer > hpEncreaseInterval)
        {
            hpTimer = 0;
        }
    }
    void oxygenDecrease()
    {
        oxygenTimer += Time.deltaTime;
        if (oxygenTimer > oxygenDecreaseInterval)
        {
            oxygenTimer = 0;
            pStats.ChangeOxygenBy(1);
        }

    }
    void energyDecrease()
    {
        energyTimer += Time.deltaTime;
        if (energyTimer > energyDecreaseInterval)
        {
            energyTimer = 0;
            pStats.ChangeEnergyBy(1);
        }

    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
