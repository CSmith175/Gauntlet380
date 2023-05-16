using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyParent
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (collision.gameObject.GetComponent<Player>() != null)
                AttackPlayer(collision.gameObject.GetComponent<Player>());
        }
    }

    protected virtual void AttackPlayer(Player player)
    {
        if (attacking)
            return;
        else
        {
            StartCoroutine(AttackCooldown());
            //Take the player reference and deal damage to that specifc player
            player.ReactToShot(stats.enemyMeleeDamage, gameObject);
        }
    }
}
