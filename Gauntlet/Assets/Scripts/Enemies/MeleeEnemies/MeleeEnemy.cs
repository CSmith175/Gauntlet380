using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyParent
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (collision.gameObject.GetComponent<Player>() != null)
                AttackPlayer(collision.gameObject.GetComponent<Player>());
        }
    }

    protected virtual void AttackPlayer(Player player)
    {

    }
}
