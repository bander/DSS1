using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {
    #region Singleton
    public static PlayerInteractions instance;
    void Awake()
    {
        instance = this;
    }
    #endregion


    List<Interactable> inters = new List<Interactable>();
    List<Interactable> enemies = new List<Interactable>();
    public List<Interactable> GetInters() { return inters; }

    PlayerControl pControl;

    void Start()
    {
        pControl = PlayerControl.instance;
    }

    public void addInteractables(Interactable inter)
    {
        if (inters.Count == 0)
        {
            MenuScript.instance.use.SetActive(true);
        }
        inters.Add(inter);
    }
    public void removeInteractables(Interactable inter)
    {
        inters.Remove(inter);
        if (inters.Count == 0)
        {
            MenuScript.instance.use.SetActive(false);
        }
    }
    public Interactable FindNearestItemToPlayer()
    {
        if (inters.Count == 0) return null;

        Interactable inter = inters[0];
        float dist = 20;
        float angle = 360;
        foreach (Interactable _inter in inters)
        {
            float newDist = (pControl.transform.position - _inter.transform.position).magnitude;
            if (dist > newDist)
            {
                dist = newDist;
                if(dist>0.7f)
                    inter = _inter;
            }
            if (newDist < 0.7f)
            {
                float newAngle = Vector3.Angle(transform.forward, _inter.transform.position - transform.position);
                if (newAngle < angle)
                {
                    angle = newAngle;
                    inter = _inter;
                }
            }
        }
        return inter;
    }

    public void interactWithItem()
    {
        pControl.GoToPickup();
    }

    public void AddEnemy(Interactable enemy)
    {
        if (enemies.Count == 0)
        {
            MenuScript.instance.attack.Activate();
            CanvasController.instance.StarttrackEnemy();
        }
        enemies.Add(enemy);
    }
    public void RemoveEnemy(Interactable enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            MenuScript.instance.attack.Activate(false);
            CanvasController.instance.StarttrackEnemy(false);
        }
    }
    public Interactable FindNearestEnemy()
    {
        if (enemies.Count == 0)
        {
            return null;
        }
        if (enemies.Count == 1 && !enemies[0].GetComponent<CharacterStats>().dead)
        {
            return enemies[0];
        }
        Interactable inter = enemies[0];
        float dist = (pControl.transform.position - enemies[0].transform.position).magnitude;
        for (int i = 1; i < enemies.Count; i++)
        {
            if (!enemies[i].GetComponent<CharacterStats>().dead)
            {
                float newDist = (pControl.transform.position - enemies[i].transform.position).magnitude;
                if (dist > newDist)
                {
                    inter = enemies[i];
                    dist = newDist;
                }
            }
            
        }
        return inter;
    }

}
