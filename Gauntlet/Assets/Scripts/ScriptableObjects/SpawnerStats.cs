using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawner Stats", fileName = "NewSpawnerStats", order = 0)]
public class SpawnerStats : ScriptableObject
{
    public float enemySpawnRate;
    public float spawnerBaseHealth;
    public int pointValue;
    public GameObject spawnerEnemyPrefab;
}
