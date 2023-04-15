using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour, IEnemy
{
    public EnemyStats stats;
    public float detectionRange = 100f;
    public LayerMask playerLayerMask = 3;

    protected GameObject closestPlayer;

    private Rigidbody rb;
    protected bool chasingPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void OnEnable()
    {
        InvokeRepeating("MoveTowardsNearestPlayer", 0.3f, 0.3f);
    }

    protected void OnDisable()
    {
        CancelInvoke("MoveTowardsNearestPlayer");
    }

    protected virtual void MoveTowardsNearestPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRange, playerLayerMask);
        float minDistance = detectionRange;
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

    public void OnDeath()
    {
        //Give points to the player and despawn self
    }

    public void ReactToShot(int shotDamage)
    {
        //Take damage from the player
    }
}
