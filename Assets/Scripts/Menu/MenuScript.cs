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

    //PlayerController pController;
    PlayerControl pControl;

    public Sprite runIcon;
    public Sprite walkIcon;
    public GameObject RunWalkButton;

    public GameObject use;
    public AttackButton attack;
    public FastButton fast;

    void Start()
    {
        pControl = PlayerControl.instance;
    }

    public void ClickUse()
    {
        PlayerInteractions.instance.interactWithItem();
    }

    /// <summary>
    /// ///////   Attack Button 
    /// </summary>
    public void AttackButton()
    {
        //PlayerManager.instance.player.GetComponent<PlayerController>().StartTrackCurrentEnemy(FindNearestEnemy());
        PlayerManager.instance.player.GetComponent<PlayerControl>().Attack();
    }
    public void AttackStart()
    {
        //PlayerManager.instance.player.GetComponent<PlayerController>().StartTrackCurrentEnemy(FindNearestEnemy());
        PlayerManager.instance.player.GetComponent<PlayerControl>().AttackStart();
    }
    public void AttackStop()
    {
        //PlayerManager.instance.player.GetComponent<PlayerController>().StartTrackCurrentEnemy(FindNearestEnemy());
        PlayerManager.instance.player.GetComponent<PlayerControl>().AttackStop();
    }


    public void UpdateFastSlot()
    {
        fast.UpdateImage();
        attack.UpdateImage();
    }
    public void UseFast()
    {
        InvManager.instance.invents[1].SwitchItems(5,1,1,1);
        InvManager.instance.OnInvChangedCallback.Invoke();
    }
}