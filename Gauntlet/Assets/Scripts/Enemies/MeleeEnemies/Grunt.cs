using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : MeleeEnemy
{
    protected override void AttackPlayer(Player player)
    {
        //Take the player reference and deal damage to that specifc player
        player.ReactToShot(stats.enemyMeleeDamage, gameObject);
    }
}
