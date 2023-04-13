using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnerStats spawnerStats;

    private Vector3 spawnBoxHalfSize = new Vector3(0.5f, 0.5f, 0.5f);
    private bool enemySpawned = false;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnerStats.enemySpawnRate, spawnerStats.enemySpawnRate);
    }

    private void SpawnEnemy()
    {
        enemySpawned = false;
        Vector3 originalOffset = new Vector3(transform.position.x-2, transform.position.y, transform.position.z - 2);
        //Iterates through z axis
        for (int i = 0; i < 3; i++)
        {
            originalOffset.z++;
            //Iterates through x axis
            for (int ii = 0; ii < 3; ii++)
            {
                originalOffset.x++;
                Debug.Log("Checking coordinate: " + originalOffset);
                Collider[] hitColliders = Physics.OverlapBox(transform.localPosition + originalOffset, spawnBoxHalfSize);
                if (Physics.OverlapBox(transform.localPosition + originalOffset, spawnBoxHalfSize).Length == 0)
                {
                    Debug.Log("Spot clear, spawning enemy");
                    GameObject enemy = Instantiate(spawnerStats.spawnerEnemyPrefab);
                    enemy.transform.position = originalOffset;
                    enemySpawned = true;
                    return;
                }
                else
                {
                    Debug.Log(originalOffset + " blocked by " + hitColliders[0].name);
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawCube(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z - 1), Vector3.one * 0.95f);
    //}
}
