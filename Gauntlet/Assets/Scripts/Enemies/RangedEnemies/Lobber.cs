using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobber : RangedEnemy
{
    protected override void EnemyShoot()
    {
        if (projectilePrefab != null)
        {
            projectile = ObjectPooling.PullObjectFromPool(projectilePrefab);
            projectile.GetComponent<Projectile>().InitilizeProjectile(gameObject, stats.enemyShotDamage, EntityType);

            projectile.transform.position = transform.position;
            projectile.GetComponent<LobberProjectile>().IntializeLob(closestPlayer.gameObject.transform.position);
        }
    }
}
