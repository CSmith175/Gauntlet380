using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProjectile : MonoBehaviour
{
    public float projectileSpeed = 1f;
    public EnemyStats stats;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            //other.gameObject.GetComponent<Player>().ReactToShot(stats.enemyShotDamage);
            Debug.Log("Oh ouch, player shot!");
            Destroy(this.gameObject);
        }
        else if(other.gameObject.layer != 6)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }
}
