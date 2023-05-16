using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour, IEnemy
{
    public ProjectileSourceType EntityType
    {
        get { return ProjectileSourceType.Enemy; }
    }

    public bool isVisible = false;
    public EnemyStats stats;
    public LayerMask playerLayerMask = 3;

    protected GameObject closestPlayer;

    private Rigidbody rb;
    private float detectionRange = 100f;
    private int currentHP;
    protected bool attacking = false;
    protected bool chasingPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void OnEnable()
    {
        currentHP = stats.enemyMaxHealth;
        InvokeRepeating("MoveTowardsNearestPlayer", 0.3f, 0.3f);
    }

    protected void OnDisable()
    {
        CancelInvoke("MoveTowardsNearestPlayer");
    }

    //Flips bool for when the enemy is visible to the main camera
    private void OnBecameVisible()
    {
        isVisible = true;
    }
    private void OnBecameInvisible()
    {
        isVisible = false;
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

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        attacking = false;
    }

    public void OnDeath()
    {
        gameObject.SetActive(false);
    }

    public void ReactToShot(int shotDamage, GameObject shotSourceEntity)
    {
        currentHP -= shotDamage;
        if (currentHP <= 0)
        {
            OnDeath();
            shotSourceEntity.GetComponent<Player>().PlayerStats.IncrementPlayerStat(PlayerStatCategories.Score, stats.enemyPointValue);
        }
    }
}
