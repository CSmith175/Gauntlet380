using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MeleeEnemy
{
    protected virtual void AttackPlayer(Player player)
    {
        //Take health directly from the player, bypassing armor
        //player.playerStats.playerStatValues.health -= stats.enemyMeleeDamage
    }
}
