using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Stats", fileName = "NewEnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public int enemyMoveSpeed;
    public int enemyMeleeDamage;
    public int enemyShotDamage;
    public int enemyEngageDistance;
    public int enemyPointValue;
    public int enemyMaxHealth;
}
