using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable {
    PlayerManager playerManager;
    CharacterStats stats;



    public string enemyName = "Enemy1";
    void Start()
    {
        playerManager = PlayerManager.instance;
        stats = GetComponent<CharacterStats>();
    }
    public override void Interact()
    {
        base.Interact();

        CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        if (playerCombat != null)
        {
            playerCombat.Attack(stats);
        }
    }
}
