using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MeleeEnemy
{
    protected override void AttackPlayer(Player player)
    {
        //Take the player reference and deal damage to that specifc player
            //player.ReactToShot(stats.enemyMeleeDamage);

        //Set self to false
        gameObject.SetActive(false);
    }
}
