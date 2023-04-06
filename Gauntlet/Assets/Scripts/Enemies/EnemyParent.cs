using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    public EnemyStats stats;
    public float detectionRange = 100f;
    public LayerMask playerLayerMask = 3;

    private Rigidbody rb;
    private bool chasingPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InvokeRepeating("MoveTowardsNearestPlayer", 0.3f, 0.3f);
    }

    private void OnDisable()
    {
        CancelInvoke("MoveTowardsNearestPlayer");
    }

    protected void MoveTowardsNearestPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRange, playerLayerMask);
        float minDistance = 100f;
        GameObject closestPlayer = null;
        //Debug.Log("Number of players found: " + players.Length);

        if (players.Length <= 0)
        {
            chasingPlayer = false;
            return;
        }
        else
            chasingPlayer = true;

        foreach (Collider player in players)
        {
            if(Vector3.Distance(transform.position, player.transform.position) <= minDistance)
            {
                //Debug.Log("Closest Player" + player.gameObject.name);
                minDistance = Vector3.Distance(transform.position,player.transform.position);
                closestPlayer = player.gameObject;
            }
        }

        if (closestPlayer != null)
        {
            //Debug.Log("Found Closest Player");
            transform.rotation = Quaternion.LookRotation(closestPlayer.transform.position - transform.position, transform.up);
        }
        else
            chasingPlayer = false;
    }

    private void FixedUpdate()
    {
        if(chasingPlayer)
            rb.velocity = transform.forward * stats.enemyMoveSpeed * Time.deltaTime;
        else
            rb.velocity = Vector3.zero;
    }
}
