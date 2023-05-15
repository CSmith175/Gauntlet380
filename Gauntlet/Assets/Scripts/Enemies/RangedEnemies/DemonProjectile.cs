using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProjectile : MonoBehaviour
{
    public float projectileSpeed = 1f;
    public EnemyStats stats;

    private void FixedUpdate()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }
}