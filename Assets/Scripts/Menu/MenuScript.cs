using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    #region Singleton
    public static MenuScript instance;
    void Awake()
    {
        instance = this;
    }


    #endregion


    PlayerController pController;

    public Sprite runIcon;
    public Sprite walkIcon;
    public GameObject RunWalkButton;

    public GameObject use;
    public AttackButton attack;

    List<Interactable> inters = new List<Interactable>();
    List<Interactable> enemies = new List<Interactable>();

    void Start()
    {
        pController = PlayerController.instance;
        pController.onMainUIUpdate += UpdateUI;
    }
    void UpdateUI()
    {
        RunWalkButton.GetComponent<Image>().sprite = (pController.isRunning()) ? runIcon : walkIcon;
    }


    public void addImteractables(Interactable inter)
    {
        if (inters.Count == 0)
        {
            use.SetActive(true);
        }
        inters.Add(inter);
    }
    public void removeImteractables(Interactable inter)
    {
        inters.Remove(inter);
        if (inters.Count == 0)
        {
            use.SetActive(false);
        }
    }
    Interactable findNearestItemToPlayer()
    {
        Interactable inter = inters[0];
        float dist = (pController.transform.position - inters[0].transform.position).magnitude;
        for (int i = 1; i < inters.Count; i++)
        {
            float newDist = (pController.transform.position - inters[1].transform.position).magnitude;
            if (dist > newDist)
            {
                inter = inters[i];
                dist = newDist;
            }
        }
        return inter;
    }

    public void interactWithItem()
    {
        if (inters.Count > 0)
        {
            Interactable inter;
            if (inters.Count == 1)
            {
                inter = inters[0];
            }
            else
            {
                inter = findNearestItemToPlayer();
            }
            pController.PickTarget(inter);
        }
    }
    
    public void AddEnemy(Interactable enemy)
    {
        if (enemies.Count == 0)
        {
            attack.Activate();
            CanvasController.instance.StarttrackEnemy();
        }
        enemies.Add(enemy);
    }
    public void RemoveEnemy(Interactable enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            attack.Activate(false);

        }
    }
    public Interactable FindNearestEnemy()
    {
        if (enemies.Count == 0)
        {
            return null;
        }
        if (enemies.Count == 1)
        {
            return enemies[0];
        }
        Interactable inter = enemies[0];
        float dist = (pController.transform.position - enemies[0].transform.position).magnitude;
        for (int i = 1; i < enemies.Count; i++)
        {
            float newDist = (pController.transform.position - enemies[1].transform.position).magnitude;
            if (dist > newDist)
            {
                inter = enemies[i];
                dist = newDist;
            }
        }
        return inter;
    }

    public void AttackButton()
    {
        PlayerManager.instance.player.GetComponent<PlayerController>().StartAttackCurrentEnemy(FindNearestEnemy());
    }
}