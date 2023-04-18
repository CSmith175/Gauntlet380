using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyParent
{
    public GameObject projectilePrefab;
    protected float rangedAttackSpeed = 2f;
    private GameObject projectile;

    private bool shooting = false;

    private void OnEnable()
    {
        base.OnEnable();
        InvokeRepeating("CheckEngagementDistance", 0.3f, 0.3f);
    }

    private void OnDisable()
    {
        base.OnDisable();
        CancelInvoke("CheckEngagementDistance");
    }

    private void CheckEngagementDistance()
    {
        if (shooting || closestPlayer == null)
            return;

        if(Vector3.Distance(transform.position, closestPlayer.transform.position) <= stats.enemyEngageDistance)
        {
            InvokeRepeating("StopAndShoot", 1 / rangedAttackSpeed, 1 / rangedAttackSpeed);
            CancelInvoke("MoveTowardsNearestPlayer");
            shooting = true;
            chasingPlayer = false;
        }
    }

    private void StopAndShoot()
    {
        if(Vector3.Distance(transform.position, closestPlayer.transform.position) <= stats.enemyEngageDistance)
        {
            Debug.Log("Shooting Player");
            transform.rotation = Quaternion.LookRotation(closestPlayer.transform.position - transform.position, transform.up);
            EnemyShoot();
        }
        else
        {
            Debug.Log("Chasing Player");
            chasingPlayer = true;
            InvokeRepeating("MoveTowardsNearestPlayer", 0.3f, 0.3f);
            shooting = false;
            CancelInvoke("StopAndShoot");
        }

        //yield return new WaitForSeconds(1 / rangedAttackSpeed);
    }

    protected void EnemyShoot()
    {
        if(projectilePrefab != null)
        {
            projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<IProjectile>().AssignStatsOfProjectile(stats);
            projectile.transform.position = transform.position;
            projectile.transform.rotation = Quaternion.LookRotation(closestPlayer.transform.position - transform.position, transform.up);
        }
    }
}
