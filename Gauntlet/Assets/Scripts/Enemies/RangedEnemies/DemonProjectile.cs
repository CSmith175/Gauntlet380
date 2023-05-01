using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProjectile : MonoBehaviour, IProjectile
{
    public float projectileSpeed = 1f;
    public EnemyStats stats;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            //other.gameObject.GetComponent<Player>().ReactToShot(stats.enemyShotDamage);
            Debug.Log("Oh ouch, player shot!");
            gameObject.SetActive(false);
        }
        else if(other.gameObject.layer != 6)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    public void AssignStatsOfProjectile(EnemyStats baseStats)
    {
        stats = baseStats;
    }
}