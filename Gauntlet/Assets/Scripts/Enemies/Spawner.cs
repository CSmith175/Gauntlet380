using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour, IGameEntity
{
    public SpawnerStats spawnerStats;

    private Vector3 spawnBoxHalfSize = new Vector3(0.45f, 0.45f, 0.45f);
    private bool enemySpawned = false;
    private int currentHealth;
    public bool isVisible = false;

    public ProjectileSourceType EntityType
    {
        get { return ProjectileSourceType.Enemy; }
    }

    private void OnEnable()
    {
        currentHealth = (int)spawnerStats.spawnerBaseHealth;
        ObjectPooling.MakeNewObjectPool(spawnerStats.spawnerEnemyPrefab);
        InvokeRepeating("SpawnEnemy", spawnerStats.enemySpawnRate, spawnerStats.enemySpawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke("SpawnEnemy");
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }
    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private void SpawnEnemy()
    {
        enemySpawned = false;
        Vector3 originalOffset = new Vector3(transform.position.x-2f, transform.position.y, transform.position.z - 2f);
        //Iterates through z axis
        for (int i = 0; i < 3; i++)
        {
            if (enemySpawned)
                return;
            originalOffset.z++;

            //Reset x axis
            originalOffset.x = transform.position.x - 2f;
            //Iterates through x axis
            for (int ii = 0; ii < 3; ii++)
            {
                originalOffset.x++;
                if(!Physics.CheckBox(originalOffset, spawnBoxHalfSize))
                {
                    //Debug.Log("Pulling object from pool");
                    GameObject enemy = ObjectPooling.PullObjectFromPool(spawnerStats.spawnerEnemyPrefab);
                    if(enemy != null)
                    {
                        enemy.transform.position = originalOffset;
                        enemySpawned = true;
                    }
                    return;
                }
            }
        }
    }

    public void ReactToShot(int shotDamage, GameObject shotSourceEntity)
    {
        currentHealth -= shotDamage;
        if(currentHealth <= 0)
        {
            Player killingPlayer;
            shotSourceEntity.TryGetComponent<Player>(out killingPlayer);
            if(killingPlayer != null)
            {
                killingPlayer.PlayerStats.IncrementPlayerStat(PlayerStatCategories.Score, spawnerStats.pointValue);
            }

            gameObject.SetActive(false);
        }
    }
}
