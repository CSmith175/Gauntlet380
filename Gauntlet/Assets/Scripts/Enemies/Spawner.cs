using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnerStats spawnerStats;

    private Vector3 spawnBoxHalfSize = new Vector3(0.45f, 0.45f, 0.45f);
    private bool enemySpawned = false;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnerStats.enemySpawnRate, spawnerStats.enemySpawnRate);
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
                    GameObject enemy = Instantiate(spawnerStats.spawnerEnemyPrefab, transform);
                    enemy.transform.position = originalOffset;
                    enemySpawned = true;
                    return;
                }
            }
        }
    }

}
